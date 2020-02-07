using Flurl.Http;
using MyScoreApi.Action;
using MyScoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyScoreApi.Function
{
    partial class MatchInfomation 
    {
        /// <summary>
        /// Ключ
        /// </summary>
        private static string _xFSign { get; set; }
        private static string _dec { get; set; }

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

                string response = await client.Request("https://www.myscore.com.ua/x/js/core.js").GetStringAsync();

                _xFSign = Regex.Match(response.ToString(), @"\|utilTran\|(.*?)\|").Groups[1].Value;
                _dec = Regex.Match(response.ToString(), @"\|Dec\|(.*?)\|").Groups[1].Value;
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
            await ParsingXFSignAsync();

            FlurlClient client = new FlurlClient();

            client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            client.Headers.Add("Accept-Encoding", "gzip, deflate");
            client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            client.Headers.Add("Cache-Control", "max-age=0");
            client.Headers.Add("Host", "www.myscore.com.ua");
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
        public async Task<List<AllTotalModels>> GetMatchOverUnderAsync(MatchModels match)
        {
            await ParsingXFSignAsync();

            FlurlClient client = new FlurlClient();

            client.Headers.Add("Accept", "*/*");
            client.Headers.Add("x-geoip", "1");
            client.Headers.Add("x-fsign", _xFSign);
            client.Headers.Add("accept-language", "*");
            client.Headers.Add("x-requested-with", "XMLHttpRequest");
            client.Headers.Add("x-referer", "https://www.myscore.com.ua/match/" + match.MatchId+ "/#odds-comparison;over-under;full-time");
            client.Headers.Add("accept-encoding", "gzip, deflate, br");

            string response = await client.Request("https://d.myscore.com.ua/x/feed/"+ "d_od_" + match.MatchId + "_ru_1_eu").GetStringAsync();
            var info = Parsing.MatchOverUnder(response.ToString());
            return info;
        }
    }
}
