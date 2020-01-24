﻿using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using MyScoreMatch.Models;

namespace MyScoreMatch.Action
{
    partial class Parsing
    {
        /// <summary>
        /// Спарсить матчи с сервиса m.myscore
        /// </summary>
        /// <param name="response">строка для парсинга</param>
        /// <returns></returns>
        public static List<MatchInfoModels> ParsingMMyScore(string response)
        {
            List<MatchInfoModels> mim = new List<MatchInfoModels>();
            int number = 0;
            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            foreach(var parsing in document.QuerySelectorAll("#main>#score-data") )
            {
                foreach ( var pars in parsing.QuerySelectorAll("a") )
                {
                    mim.Add(new MatchInfoModels() { Link = "https://www.myscore.com.ua" + pars.GetAttribute("href") });
                }

                foreach ( var pars in parsing.QuerySelectorAll("span") )
                {
                    var timePars = pars.FirstChild.TextContent.Replace("'", "");

                    int? startTime = null;
                    DateTime? time = null;

                    try { startTime = int.Parse(timePars); } catch ( FormatException e ) { }
                    try { time = DateTime.Parse(timePars); } catch ( FormatException e ){ }

                    var ttsss = pars.OuterHtml;
                    var tttsss = ttsss.Contains("canceled");

                    if ( startTime != null || time != null )
                    {
                        try
                        {
                            //mim[number].DateStart = DateTime.Parse(timePars) : DateTime.Now.AddMinutes(-startTime.Value);
                            mim[number].DateStart = DateTime.Parse(timePars);
                        }
                        catch ( FormatException )
                        {
                            mim[number].DateStart = null;
                        }
                        number++;
                    }
                }
            }
            return mim;
        }        

        /// <summary>
        /// Спарсить информацию о матчей
        /// </summary>
        /// <param name="response">исходный код страницы</param>
        /// <returns></returns>
        public static MatchInfoModels ParsingMatchInfo(string response)
        {
            MatchInfoModels matchInfo = new MatchInfoModels();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            matchInfo.Command1.Name = document.QuerySelectorAll(".participant-imglink>img")[0].GetAttribute("alt");
            matchInfo.Command2.Name = document.QuerySelectorAll(".participant-imglink>img")[1].GetAttribute("alt");

            if ( document.QuerySelectorAll("span.scoreboard").Count() > 0 )
            {
                matchInfo.Command1.Goal = document.QuerySelectorAll(".scoreboard")[0].TextContent;
                matchInfo.Command2.Goal = document.QuerySelectorAll(".scoreboard")[1].TextContent;
            }
            matchInfo.Country = document.QuerySelector(".description__country").FirstChild.TextContent;
            matchInfo.Liga = document.QuerySelector(".description__country>a").TextContent;

            var tts = document.QuerySelector("#utime").TextContent;
            try
            {
                matchInfo.DateStart = DateTime.Parse(document.QuerySelector("#utime").TextContent);
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
        public static Dictionary<double, List<TotalModels>> ParsingMatchOverUnder(string response)
        {
            Dictionary<double, List<TotalModels>> bk = new Dictionary<double, List<TotalModels>>();

            HtmlParser hp = new HtmlParser();
            var document = hp.Parse(response);

            foreach(var doc in document.QuerySelectorAll("#block-under-over-ft>table") )
            {

                string total = doc.QuerySelectorAll("tbody>tr.odd>td")[1].TextContent;
                List<TotalModels> totalInfo = new List<TotalModels>();
                foreach ( var tbody in doc.QuerySelectorAll("tbody>tr") )
                {
                    string bkName = tbody.QuerySelector("td.bookmaker>div>a").GetAttribute("title");
                    string more = tbody.QuerySelectorAll("td")[2].QuerySelector("span").TextContent;
                    string less = tbody.QuerySelectorAll("td")[3].QuerySelector("span").TextContent;

                    totalInfo.Add(new TotalModels()
                    {
                        BkName = bkName,
                        More = double.Parse(more == "-" ? "0" : more),
                        Less = double.Parse(less == "-" ? "0" : less),
                    });

                }

                bk.Add(double.Parse( total), totalInfo);
            }

            return bk;
        }
    }
}
