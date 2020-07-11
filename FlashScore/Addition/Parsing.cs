using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using FlashScore.Models;
using FlashScore.Exception;
using System.Globalization;
using FlashScore.Models.Coefficient;
using FlashScore.Models.H2H;

namespace FlashScore.Action
{
    partial class Parsing
    {
        /// <summary>
        /// Спарсить матчи с сервиса m.FlashScore
        /// </summary>
        /// <param name="response">строка для парсинга</param>
        /// <returns></returns>
        public static List<MatchModels> MFlashScore(string response, bool addOneHour = false)
        {
            List<MatchModels> mim = new List<MatchModels>();
            int number = 0;

            var document = new HtmlParser().Parse(response);
            foreach ( var pars in document.QuerySelectorAll("#main>#score-data>a") )
            {
                mim.Add(new MatchModels() {
                    Link = "https://www.FlashScore.com.ua" + pars.GetAttribute("href"),
                    Match = new MatchInfoModels(),
                });
            }

            number = 0;
            foreach ( var pars in document.QuerySelectorAll("#main>#score-data>span") )
            {
                var timePars = pars.FirstChild.TextContent.Replace("'", "");

                DateTime? time;

                if (timePars.Contains(":"))
                {
                    if (addOneHour)
                        time = DateTime.Parse(timePars).AddHours(1);
                    else
                        time = DateTime.Parse(timePars);
                }
                else
                {
                    int checkNumb = 0;
                    try { checkNumb = int.Parse(timePars); } catch { }

                    if (checkNumb == 0)
                        time = DateTime.Now.AddMinutes(-45);
                    else
                        time = DateTime.Now.AddMinutes(-Convert.ToInt32(timePars));
                }

                if (FlashScoreApi.NewInfo == Enums.DayInfo.Tomorrow)
                    time = time.Value.AddDays(1);
                else if (FlashScoreApi.NewInfo == Enums.DayInfo.Yesterday)
                    time = time.Value.AddDays(-1);

                mim[number].Match.DateStart = time;
                number++;
            }

            return mim;
        }

        private const string None = "";
        private const string Completed = "Завершен";
        private const string Canceled = "Отменен";
        private const string Absence = "Неявка";
        private const string SeriesOfPinal = "После серии пенальти";
        private const string Moved = "Перенесен";
        private const string TechDefeat = "Тех. поражение";
        private const string AfterExtraTime = "После дополнительного времени";

        /// <summary>
        /// Спарсить информацию о матчей
        /// </summary>
        /// <param name="response">исходный код страницы</param>
        /// <returns></returns>
        public static MatchModels MatchInfo(string response)
        {

            MatchModels matchInfo = new MatchModels();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            matchInfo.Match.Command1.Name = document.QuerySelectorAll(".participant-imglink>img")[0].GetAttribute("alt");
            matchInfo.Match.Command2.Name = document.QuerySelectorAll(".participant-imglink>img")[1].GetAttribute("alt");

            if ( document.QuerySelectorAll("span.scoreboard").Count() > 0 )
            {
                matchInfo.Match.Command1.Goal = int.Parse( document.QuerySelectorAll(".scoreboard")[0].TextContent);
                matchInfo.Match.Command2.Goal = int.Parse( document.QuerySelectorAll(".scoreboard")[1].TextContent);
            }
            matchInfo.Match.Country = document.QuerySelector(".description__country").FirstChild.TextContent.Replace(": ", "");
            matchInfo.Match.Liga = document.QuerySelector(".description__country>a").TextContent;

            var tts = document.QuerySelector("#utime").TextContent;

            try
            {
                var date = DateTime.Parse(document.QuerySelector("#utime").TextContent);
                matchInfo.Match.DateStart = date;
            }
            catch (FormatException)
            {
                matchInfo.Match.DateStart = null;
            }

            var resultMatch = document.QuerySelector(".info-status.mstat").TextContent.Trim();
            if (resultMatch == None) matchInfo.Result = Enums.ResultMatch.None;
            if (resultMatch == Completed) matchInfo.Result = Enums.ResultMatch.Completed;
            if (resultMatch == Canceled) matchInfo.Result = Enums.ResultMatch.Calceled;
            if (resultMatch == Absence) matchInfo.Result = Enums.ResultMatch.Absence;
            if (resultMatch == SeriesOfPinal) matchInfo.Result = Enums.ResultMatch.SeriesOfPinal;
            if (resultMatch == Moved) matchInfo.Result = Enums.ResultMatch.Moved;
            if (resultMatch == TechDefeat) matchInfo.Result = Enums.ResultMatch.TechDefeat;
            if (resultMatch == AfterExtraTime) matchInfo.Result = Enums.ResultMatch.AfterExtraTime;

            return matchInfo;
        }

        /// <summary>
        /// Спарсить информаию о более/менее
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static List<AllTotalModels> CoeffBM(string response)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            List<AllTotalModels> allTotal = new List<AllTotalModels>();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            foreach(var doc in document.QuerySelectorAll("#block-under-over-ft>table") )
            {
                string total = doc.QuerySelectorAll("tbody>tr.odd>td")[1].TextContent;

                if ( total == null ) throw new ErrorOverUnderException("Параметр total - null");

                foreach ( var tbody in doc.QuerySelectorAll("tbody>tr") )
                {
                    string bkName = tbody.QuerySelector("td.bookmaker>div>a").GetAttribute("title");
                    string more = tbody.QuerySelectorAll("td")[2].QuerySelector("span").TextContent;
                    string less = tbody.QuerySelectorAll("td")[3].QuerySelector("span").TextContent;

                    if ( more == null  ) throw new ErrorOverUnderException("Параметр more - null");
                    if ( less == null ) throw new ErrorOverUnderException("Параметр less - null");

                    allTotal.Add(new AllTotalModels()
                    {
                        Total = double.Parse(total),
                        BkName = bkName,
                        More = double.Parse(more == "-" ? "0" : more),
                        Less = double.Parse(less == "-" ? "0" : less),
                    });

                }
            }

            return allTotal;
        }

        public static List<AllTotalModels> CoeffFDS(string response)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            List<AllTotalModels> allTotal = new List<AllTotalModels>();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            foreach ( var tbody in document.QuerySelectorAll("#block-1x2>#block-1x2-ft>#odds_1x2>tbody>tr") )
            {
                string bkName = tbody.QuerySelector("td.bookmaker>div>a").GetAttribute("title");

                string first = tbody.QuerySelectorAll("td.kx")[0].QuerySelector("span").TextContent;
                string two = tbody.QuerySelectorAll("td.kx")[1].QuerySelector("span").TextContent;
                string x = tbody.QuerySelectorAll("td.kx")[2].QuerySelector("span").TextContent;

                if ( first == null ) throw new ErrorOverUnderException("Параметр first - null");
                if ( two == null ) throw new ErrorOverUnderException("Параметр two - null");
                if ( x == null ) throw new ErrorOverUnderException("Параметр x - null");

                allTotal.Add(new AllTotalModels()
                {
                    BkName = bkName,
                    First = double.Parse(first == "-" ? "0" : first),
                    Two = double.Parse(two == "-" ? "0" : two),
                    X = double.Parse(x == "-" ? "0" : x),

                });

            }
            return allTotal;
        }


        /// <summary>
        /// Получить информацию о голах матчей
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static H2HModels H2HInfo(string response)
        {
            H2HModels h2hInfo = new H2HModels();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            int number = 0;

            foreach ( var overall in document.QuerySelectorAll("#tab-h2h-overall>.h2h-wrapper>table") )
            {
                var lastGame = new H2HTotalModels();
                var h2hMatch = new H2HMatchModels();

                var name = overall.QuerySelector("thead>tr>td").TextContent.Split(':');
                lastGame.Name = string.Join(" ",name.Skip(1)).TrimStart();
                lastGame.Match = new List<H2HMatchModels>();

                foreach ( var matchInfo in overall.QuerySelectorAll("tbody>tr") )
                {
                    var matchOuthHtml = matchInfo.OuterHtml;
                    if ( matchInfo.Matches("tr.hid") || matchOuthHtml.Contains("apology last") ) break;

                    var infoGoal = matchInfo.QuerySelector("td>span.score>strong").TextContent;
                    string[] goal = null;
                    if ( infoGoal != "-" )
                        goal = infoGoal.Split(new char[] { ':', ' ' });
                    else
                        goal = new string[] { "0", "0", "0", "0" };
                    var nameCommand = matchInfo.QuerySelectorAll("td.name");

                    h2hMatch = new H2HMatchModels()
                    {
                        Command1 = new CommandModels()
                        {
                            Goal = int.Parse(goal.First()),
                            Name = nameCommand[0].TextContent,
                        },
                        Command2 = new CommandModels()
                        {
                            Goal = int.Parse(goal.Last()),
                            Name = nameCommand[1].TextContent
                        },
                    };
                    lastGame.Match.Add(h2hMatch);
                }

                if(number == 0 )
                    h2hInfo.LastGameCommand1 = lastGame;
                else if(number == 1)
                    h2hInfo.LastGameCommand2 = lastGame;
                else
                    h2hInfo.Confrontation = lastGame;

                number++;
            }

            return h2hInfo;
        }

    }
}
