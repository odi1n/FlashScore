using FlashScore.Action;
using FlashScore.Exception;
using FlashScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using FlashScore.Function;

namespace FlashScore
{
    public class FlashScoreApi
    {
        /// <summary>
        /// Матчи которые будут сегодня
        /// </summary>
        public List<MatchModels> Matches { get; set; } = new List<MatchModels>();
        /// <summary>
        /// Новые матчи
        /// </summary>
        public static bool NewInfo { get; set; }

        public FlashScoreApi()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        /// <summary>
        /// Получить матчи с m.FlashScore.com.ua
        /// </summary>
        /// <param name="newInfo">false - сегодня, true - завтра</param>
        /// <returns></returns>
        public async Task<List<MatchModels>> GetAllMatchesAsync(bool newInfo = false)
        {
            NewInfo = newInfo;

            FlurlClient client = new FlurlClient();
            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add("Host", "m.FlashScore.com.ua");
            client.Headers.Add("Upgrade-Insecure-Requests", "1");

            string response = null;
            if ( newInfo )
                response = await client.Request("https://m.FlashScore.com.ua/" + "?d=1").GetStringAsync();
            else
                response = await client.Request("https://m.FlashScore.com.ua/").GetStringAsync();

            Matches = Parsing.MFlashScore(response).ToList();
            return Matches;
        }
    }
}
