using Leaf.xNet;
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
                //break;
            }
            //mim.RemoveAll(x => x.DateStart == null);
            return mim;
        }        
    }
}
