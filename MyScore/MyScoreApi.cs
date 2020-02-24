﻿using MyScore.Action;
using MyScore.Exception;
using MyScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MyScore.Function;

namespace MyScore
{
    public class MyScoreApi
    {
        /// <summary>
        /// Матчи которые будут сегодня
        /// </summary>
        public List<MatchModels> Matches { get; set; } = new List<MatchModels>();
        /// <summary>
        /// Получать новые матчи
        /// </summary>
        public static bool GetNewInfo { get; set; } = false;
        /// <summary>
        /// Получать новые матчи если новый день
        /// </summary>
        private FlurlClient _client { get; set; }

        public MyScoreApi()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        /// <summary>
        /// Получить матчи с m.myscore.com.ua
        /// </summary>
        /// <param name="newInfo">false - сегодня, true - завтра</param>
        /// <returns></returns>
        public async Task<List<MatchModels>> GetMatchesAsync(bool newInfo = false)
        {
            GetNewInfo = newInfo;
            _client = new FlurlClient();

            string response = null;

            _client.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            _client.Headers.Add("Accept-Encoding", "gzip, deflate");
            _client.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            _client.Headers.Add("Cache-Control", "max-age=0");
            _client.Headers.Add("Host", "m.myscore.com.ua");
            _client.Headers.Add("Upgrade-Insecure-Requests", "1");

            if ( newInfo )
                response = await _client.Request("https://m.myscore.com.ua/" + "?d=1").GetStringAsync();
            else
                response = await _client.Request("https://m.myscore.com.ua/").GetStringAsync();

            Matches = Parsing.MMyScore(response).OrderBy(x => x.DateStart).ToList();
            return Matches;
        }
    }
}