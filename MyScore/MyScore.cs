using Leaf.xNet;
using MyScoreMatch.Action;
using MyScoreMatch.Exception;
using MyScoreMatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyScoreMatch
{
    public class MyScore
    {
        /// <summary>
        /// Матчи которые будут сегодня
        /// </summary>
        public List<MatchModels> MatchesToday { get; set; } = new List<MatchModels>();
        /// <summary>
        /// Ключ
        /// </summary>
        private string XFSign { get; set; }
        /// <summary>
        /// Для запросов
        /// </summary>
        private Request _request = new Request();

        /// <summary>
        /// Получить матчи с m.myscore.com.ua на сегодня
        /// </summary>
        /// <returns></returns>
        public List<MatchModels> GetMatchesToday()
        {
            HttpRequest request = _request.httpRequest();
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            request.AddHeader("Cache-Control", "max-age=0");
            request.AddHeader("Host", "m.myscore.com.ua");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            HttpResponse response = request.Get("https://m.myscore.com.ua/");

            MatchesToday = Parsing.ParsingMMyScore(response.ToString()).OrderBy(x=>x.DateStart).ToList();

            ParsingXFSign();

            return MatchesToday;
        }

        /// <summary>
        /// Получить матчи в течении указанных минут
        /// </summary>
        /// <param name="minutes">Минуты, матчи которые получим в ближайшие 60(по умолчанию) минут</param>
        /// <returns></returns>
        public  List<MatchModels> NearestMatchesMinutes( int minutes = 60)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            return MatchesToday.Where(x => x.DateStart > DateTime.Now && x.DateStart < DateTime.Now.AddMinutes(minutes)).ToList();
        }

        /// <summary>
        /// Получить матчи в течении указанных часов
        /// </summary>
        /// <param name="hours">Часы, матчи которые получим в ближайшие 1(по умолчанию) час</param>
        /// <returns></returns>
        public List<MatchModels> NearestMatchesHours(int hours = 1)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            return MatchesToday.Where(x =>
            x.DateStart > DateTime.Now && 
            x.DateStart < DateTime.Now.AddHours(hours)).ToList();
        }

        /// <summary>
        /// Получить матчи в указанном промежутке
        /// </summary>
        /// <param name="nearestMatche">Указать часы или минуты</param>
        /// <returns></returns>
        public List<MatchModels> NearestMatches(NearestMatchesModels nearestMatche)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            if ( nearestMatche.Hours > 24 || nearestMatche.Hours < -24||
                nearestMatche.Minutes > 1440 || nearestMatche.Minutes < -1440 )
                throw new ErrorNearestMatches("Указано времени больше чем может быть в сутках");

            return MatchesToday.Where(x => 
            x.DateStart > DateTime.Now &&
            x.DateStart < DateTime.Now.AddHours(nearestMatche.Hours).AddMinutes(nearestMatche.Minutes)).ToList();
        }

        /// <summary>
        /// Ключ для получение информации о матче
        /// </summary>
        /// <returns></returns>
        private string ParsingXFSign()
        {
            HttpRequest request = _request.httpRequest();
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", " gzip, deflate, br");
            request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
            HttpResponse response = request.Get("https://www.myscore.com.ua/x/js/core.js");

            XFSign = Regex.Match(response.ToString(), @"\|server_domain\|(.*?)\|").Groups[1].Value;
            return XFSign;
        }

        /// <summary>
        /// Получить информацию о матче полностью
        /// </summary>
        /// <param name="match">Матч</param>
        /// <returns></returns>
        public MatchModels GetInfo(MatchModels match)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var matchInfo = GetMatchInfo(match);
            var overUnder = GetMatchOverUnder(match);

            match.Command1 = matchInfo.Command1;
            match.Command2 = matchInfo.Command2;
            match.Country = matchInfo.Country;
            match.Liga = matchInfo.Liga;

            if( matchInfo.DateStart != null)
                match.DateStart = matchInfo.DateStart;

            match.Bookmaker = overUnder;

            return match;
        }

        /// <summary>
        /// Получить информацию обо всех матчах которые были выбраны
        /// </summary>
        /// <param name="match">Список выбранных матчей</param>
        /// <returns></returns>
        public List<MatchModels> GetInfo(List<MatchModels> matches)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            int i = 0;
            foreach ( var match in matches )
            {
                i++;
                var matchInfo = GetMatchInfo(match);
                var overUnder = GetMatchOverUnder(match);

                match.Command1 = matchInfo.Command1;
                match.Command2 = matchInfo.Command2;
                match.Country = matchInfo.Country;
                match.Liga = matchInfo.Liga;

                if ( matchInfo.DateStart != null )
                    match.DateStart = matchInfo.DateStart;

                match.Bookmaker = overUnder;
                Console.WriteLine(i + ":" + matches.Count);
            }
            return matches;
        }

        /// <summary>
        /// Получить информацию обо всех матчах на сегодня
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool GetAllInfo()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            int i = 0;
            foreach ( var match in MatchesToday )
            {
                i++;
                var matchInfo = GetMatchInfo(match);
                var overUnder = GetMatchOverUnder(match);

                match.Command1 = matchInfo.Command1;
                match.Command2 = matchInfo.Command2;
                match.Country = matchInfo.Country;
                match.Liga = matchInfo.Liga;

                if ( matchInfo.DateStart != null )
                    match.DateStart = matchInfo.DateStart;

                match.Bookmaker = overUnder;
                Console.WriteLine(i + ":" + MatchesToday.Count);
            }
            return true;
        }

        /// <summary>
        /// Получить информацию конкретно о матче
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private MatchModels GetMatchInfo(MatchModels match)
        {
            HttpResponse response = null;
            for ( int i = 0; i < 3; i++ )
            {
                try
                {
                    HttpRequest request = _request.httpRequest();
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    request.AddHeader("Accept-Encoding", "gzip, deflate");
                    request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,af;q=0.6");
                    request.AddHeader("Cache-Control", "max-age=0");
                    request.AddHeader("Host", "www.myscore.com.ua");
                    request.AddHeader("Upgrade-Insecure-Requests", "1");
                    response = request.Get(match.Link + "#odds-comparison;over-under;full-time");
                    break;
                }
                catch ( HttpException e ) { }
            }
            MatchModels mim = Parsing.ParsingMatchInfo(response.ToString()) ;
            return mim;
        }

        /// <summary>
        /// Получить коэф, тотал, более/менее о матче
        /// </summary>
        /// <param name="match">Матч о котором нужно получить информацию</param>
        /// <returns></returns>
        private Dictionary<double, List<TotalModels>> GetMatchOverUnder(MatchModels match)
        {
            HttpResponse response = null;
            for ( int i = 0; i < 3; i++ )
            {
                try
                {
                    HttpRequest request = _request.httpRequest();
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("x-geoip", "1");
                    request.AddHeader("x-fsign", XFSign);
                    request.AddHeader("accept-language", "*");
                    request.AddHeader("x-requested-with", "XMLHttpRequest");
                    request.AddHeader("x-referer", "https://www.myscore.com.ua/match/" + match.MatchId + "/#odds-comparison;over-under;full-time");
                    request.AddHeader("accept-encoding", "gzip, deflate, br");
                    response = request.Get("https://d.myscore.com.ua/x/feed/d_od_" + match.MatchId + "_ru_1_eu");
                    break;
                }
                catch ( HttpException e ) { }
            }
            var info = Parsing.ParsingMatchOverUnder(response.ToString());
            return info;

        }
    }
}
