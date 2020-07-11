using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashScore;
using System.IO;
using FlashScore.Enums;

namespace FlashScore_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            Console.ReadKey();
        }

        async static void Test()
        {
            FlashScoreApi FlashScore = new FlashScoreApi();
            var matches = await FlashScore.GetAllMatchesAsync(DayInfo.Today);
            var info = await matches.GetInfoAsync(true, true, true, true);
            foreach (var match in info)
            {
                string test = "";

                test += "name: " + match.Match.Name + " | "+
                    "time: " + match.Match.DateStart + " | "+
                    "liga: " + match.Match.Liga + " | "+
                    "link: " + match.Link + " | ";

                //foreach (var matchTotal in match.Coefficient.BM)
                //{
                //    test += "key:" + matchTotal.Total + "\n";
                //    test += "bk:" + matchTotal.BkName + "\n";
                //    test += "more:" + matchTotal.More + "\n";
                //    test += "less:" + matchTotal.More + "\n";
                //}
                Console.WriteLine(test + "\n");
            }
        }
    }
}
