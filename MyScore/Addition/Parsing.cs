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
                foreach (var pars in parsing.QuerySelectorAll("span") )
                {
                    try
                    {
                        mim.Add(new MatchInfoModels() { DateStart = DateTime.Parse(pars.TextContent) });
                    }
                    catch ( FormatException )
                    {
                        mim.Add(new MatchInfoModels() { DateStart = null });
                    }
                }

                foreach ( var pars in parsing.QuerySelectorAll("a") )
                {
                    if( mim[number].DateStart != null )
                        mim[number].Link = "https://www.myscore.com.ua" + pars.GetAttribute("href");

                    number++;
                }

            }
            mim.RemoveAll(x => x.DateStart == null);
            return mim.OrderBy(x=>x.DateStart).ToList();
        }        
    }
}
