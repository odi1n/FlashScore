using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyScoreMatch.Action;
using MyScoreMatch;

namespace MyScore_test
{
    class Program
    {
        static void Main(string[] args)
        {
            MyScore query = new MyScore();
            var parsing = query.GetMatchesToday();


            var infoOverUnder = query.GetAllInfo();

            Console.Clear();

            string test = "";
            foreach ( var matches in query.MatchesToday )
            {
                test += ("name: " + matches.Name + "\n");
                test +=("time: " + matches.DateStart + "\n");
                test += ("liga: " + matches.Liga + "\n");
                test += ("link: " + matches.Link + "\n");
                foreach ( var match in matches.Bookmaker )
                {
                    test += ("key:" + match.Key + "\n");
                    foreach ( var val in match.Value )
                    {
                        test += ("info: " + val.BkName + " | " + val.Less + " | " + val.More + "\n");
                    }
                }
                test += "\n";
            }

            Console.ReadKey();
        }
    }
}
