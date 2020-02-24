using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using MyScore.Models;
using MyScore.Exception;
using System.Globalization;

namespace MyScore.Action
{
    partial class Parsing
    {
        /// <summary>
        /// Спарсить матчи с сервиса m.myscore
        /// </summary>
        /// <param name="response">строка для парсинга</param>
        /// <returns></returns>
        public static List<MatchModels> MMyScore(string response)
        {
            List<MatchModels> mim = new List<MatchModels>();
            int number = 0;

            var document = new HtmlParser().Parse(response);
            foreach ( var pars in document.QuerySelectorAll("#main>#score-data>a") )
            {
                mim.Add(new MatchModels() { Link = "https://www.myscore.com.ua" + pars.GetAttribute("href") });
            }

            number = 0;
            foreach ( var pars in document.QuerySelectorAll("#main>#score-data>span") )
            {
                var timePars = pars.FirstChild.TextContent.Replace("'", "");

                DateTime? time;

                if ( timePars.Contains(":") )
                    time = DateTime.Parse(timePars).AddHours(1);
                else
                {
                    int checkNumb = 0;
                    try { checkNumb = int.Parse(timePars); } catch { }

                    if ( checkNumb == 0 )
                        time = DateTime.Now.AddMinutes(-45);
                    else
                        time = DateTime.Now.AddMinutes(-Convert.ToInt32(timePars));
                }

                if ( MyScoreApi.GetNewInfo )
                    time =time.Value.AddDays(1);

                mim[number].DateStart = time;
                number++;
            }

            return mim;
        }

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

            matchInfo.Command1.Name = document.QuerySelectorAll(".participant-imglink>img")[0].GetAttribute("alt");
            matchInfo.Command2.Name = document.QuerySelectorAll(".participant-imglink>img")[1].GetAttribute("alt");

            if ( document.QuerySelectorAll("span.scoreboard").Count() > 0 )
            {
                matchInfo.Command1.Goal = int.Parse( document.QuerySelectorAll(".scoreboard")[0].TextContent);
                matchInfo.Command2.Goal = int.Parse( document.QuerySelectorAll(".scoreboard")[1].TextContent);
            }
            matchInfo.Country = document.QuerySelector(".description__country").FirstChild.TextContent;
            matchInfo.Liga = document.QuerySelector(".description__country>a").TextContent;

            try
            {
                var date = DateTime.Parse(document.QuerySelector("#utime").TextContent);
                matchInfo.DateStart = date;
            }
            catch (FormatException)
            {
                matchInfo.DateStart = null;
            }
            return matchInfo;
        }

        /// <summary>
        /// Спарсить информаию о более/менее
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static List<AllTotalModels> MatchOverUnder(string response)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            List<AllTotalModels> allTotal = new List<AllTotalModels>();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            foreach(var doc in document.QuerySelectorAll("#block-under-over-ft>table") )
            {
                string total = doc.QuerySelectorAll("tbody>tr.odd>td")[1].TextContent;

                if ( total == null ) throw new ErrorOverUnderException("Параметр total - null");

                List<TotalModels> totalInfo = new List<TotalModels>();
                foreach ( var tbody in doc.QuerySelectorAll("tbody>tr") )
                {
                    string bkName = tbody.QuerySelector("td.bookmaker>div>a").GetAttribute("title");
                    string more = tbody.QuerySelectorAll("td")[2].QuerySelector("span").TextContent;
                    string less = tbody.QuerySelectorAll("td")[3].QuerySelector("span").TextContent;

                    if ( more == null  ) throw new ErrorOverUnderException("Параметр more - null");
                    if ( less == null ) throw new ErrorOverUnderException("Параметр less - null");

                    totalInfo.Add(new TotalModels()
                    {
                        BkName = bkName,
                        More = double.Parse(more == "-" ? "0" : more),
                        Less = double.Parse(less == "-" ? "0" : less),
                    });

                }

                allTotal.Add(new AllTotalModels()
                {
                    Total = double.Parse(total),
                    Info= totalInfo,
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
                var lastGame = new InfoGameModels();
                var h2hMatch = new H2HMatchModels();

                var name = overall.QuerySelector("thead>tr>td").TextContent.Split(':');
                lastGame.Name = string.Join(" ",name.Skip(1));
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
