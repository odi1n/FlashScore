using MyScoreApi.Action;
using MyScoreApi.Exception;
using MyScoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MyScoreApi.Function;

namespace MyScoreApi
{
    public class MyScore
    {
        /// <summary>
        /// Матчи которые будут сегодня
        /// </summary>
        public List<MatchModels> MatchesToday { get; set; } = new List<MatchModels>();
        /// <summary>
        /// Получать новые матчи если новый день
        /// </summary>
        private FlurlClient _client { get; set; }
        private MatchInfomation _match { get; set; } = new MatchInfomation();

        public MyScore()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        /// <summary>
        /// Получить матчи с m.myscore.com.ua
        /// </summary>
        /// <param name="newInfo">false - сегодня, true - завтра</param>
        /// <returns></returns>
        public async Task<List<MatchModels>> GetMatches(bool newInfo = false)
        {
            _client = new FlurlClient();

            _client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            _client.Headers.Add("Accept-Encoding", "gzip, deflate");
            _client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            _client.Headers.Add("Cache-Control", "max-age=0");
            _client.Headers.Add("Host", "m.myscore.com.ua");
            _client.Headers.Add("Upgrade-Insecure-Requests", "1");

            string response = null;
            if ( newInfo )
                response = await _client.Request("https://m.myscore.com.ua/" + "?d=1").GetStringAsync();
            else
                response = await _client.Request("https://m.myscore.com.ua/").GetStringAsync();

            MatchesToday = Parsing.MMyScore(response).OrderBy(x => x.DateStart).ToList();
            return MatchesToday;
        }
    }
}
