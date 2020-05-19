using Flurl.Http;
using FlashScore.Action;
using FlashScore.Models;
using FlashScore.Models.Coefficient;
using FlashScore.Models.H2H;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlashScore.Function
{
    partial class MatchInfomation 
    {
        /// <summary>
        /// Ключ
        /// </summary>
        private static string _xFSign { get; set; } = "SW9D1eZo";

        /// <summary>
        /// Ключ для получение информации о матче
        /// </summary>
        /// <returns></returns>
        private async Task<string> ParsingXFSignAsync()
        {
            if ( _xFSign == null )
            {
                FlurlClient client = new FlurlClient();
                client.Headers.Add("Accept", "*/*");
                client.Headers.Add("Accept-Encoding", " gzip, deflate, br");
                client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");

                string response = await client.Request("https://www.FlashScore.com.ua/x/js/core.js").GetStringAsync();

                _xFSign = Regex.Match(response.ToString(), @"\|utilTran\|(.*?)\|").Groups[1].Value;
            }
            return _xFSign;
        }

        /// <summary>
        /// Получить информацию конкретно о матче
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public async Task<MatchModels> GetMatchInfoAsync(MatchModels match)
        {
            FlurlClient client = new FlurlClient();

            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add("Host", "www.FlashScore.com.ua");
            client.Headers.Add("Upgrade-Insecure-Requests", "1");

            string response = await client.Request(match.Link + "#odds-comparison;over-under;full-time").GetStringAsync();
            MatchModels mim = Parsing.MatchInfo(response.ToString());
            return mim;
        }

        /// <summary>
        /// Получить коэф, тотал, более/менее о матче
        /// </summary>
        /// <param name="match">Матч о котором нужно получить информацию</param>
        /// <returns></returns>
        public async Task<string> GetPageCoefficient(MatchModels match)
        {
            FlurlClient client = new FlurlClient();

            client.Headers.Add("Accept", "*/*");
            client.Headers.Add("x-geoip", "1");
            client.Headers.Add("x-fsign", _xFSign);
            client.Headers.Add("accept-language", "*");
            client.Headers.Add("x-requested-with", "XMLHttpRequest");
            client.Headers.Add("x-referer", "https://www.FlashScore.com.ua/match/" + match.Id+ "/#odds-comparison;over-under;full-time");
            client.Headers.Add("accept-encoding", "gzip, deflate, br");

            string response = await client.Request("https://d.FlashScore.com.ua/x/feed/"+ "d_od_" + match.Id + "_ru_1_eu").GetStringAsync();
            return response;
        }

        /// <summary>
        /// Получить информацию о голах команды
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public async Task<H2HModels> GetH2H(MatchModels match)
        {
            FlurlClient client = new FlurlClient();

            client.Headers.Add("Accept", "*/*");
            client.Headers.Add("x-fsign", _xFSign);
            client.Headers.Add("accept-language", "*");
            client.Headers.Add("x-requested-with", "XMLHttpRequest");
            client.Headers.Add("x-referer", "https://www.FlashScore.com.ua/match/" + match.Id + "/#odds-comparison;over-under;full-time");
            client.Headers.Add("accept-encoding", "gzip, deflate, br");

            string response = await client.Request("https://d.FlashScore.com.ua/x/feed/" + "d_hh_" + match.Id + "_ru_1_eu").GetStringAsync();
            var info = Parsing.H2HInfo(response.ToString());
            return info;
        }

    }
}
