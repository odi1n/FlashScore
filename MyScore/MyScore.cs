using Leaf.xNet;
using MyScoreMatch.Action;
using MyScoreMatch.Exception;
using MyScoreMatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreMatch
{
    public class MyScore
    {
        /// <summary>
        /// Матчи которые будут сегодня
        /// </summary>
        public List<MatchInfoModels> MatchesToday = new List<MatchInfoModels>();
        /// <summary>
        /// Для запросов
        /// </summary>
        private Request _request = new Request();

        /// <summary>
        /// Получить матчи с m.myscore.com.ua на сегодня
        /// </summary>
        /// <returns></returns>
        public List<MatchInfoModels> GetMatchesToday()
        {
            HttpRequest request = _request.httpRequest();
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            request.AddHeader("Cache-Control", "max-age=0");
            request.AddHeader("Host", "m.myscore.com.ua");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            HttpResponse response = request.Get("https://m.myscore.com.ua/");

            MatchesToday = Parsing.ParsingMMyScore(response.ToString());
            return MatchesToday;
        }

        /// <summary>
        /// Получить матчи в течении указанных минут
        /// </summary>
        /// <param name="minutes">Минуты, матчи которые получим в ближайшие 60(по умолчанию) минут</param>
        /// <returns></returns>
        public List<MatchInfoModels> NearestMatchesMinutes(int minutes = 60)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            return MatchesToday.Where(x => x.DateStart.Value.AddMinutes(minutes) > DateTime.Now).ToList();
        }

        /// <summary>
        /// Получить матчи в течении указанных часов
        /// </summary>
        /// <param name="hours">Часы, матчи которые получим в ближайшие 1(по умолчанию) час</param>
        /// <returns></returns>
        public List<MatchInfoModels> NearestMatchesHours(int hours = 1)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            return MatchesToday.Where(x => x.DateStart.Value.AddHours(hours) > DateTime.Now).ToList();
        }

        //public string GetInfoMatch()
        //{

        //}
    }
}
