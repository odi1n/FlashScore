using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashScore;
using System.IO;

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
            var matches = await FlashScore.GetAllMatchesAsync();
            var info = await matches.GetInfoAsync();

            foreach (var match in info)
            {
                string test = "";

                test += "name: " + match.Match.Name + "\n";
                test += "time: " + match.Match.DateStart + "\n";
                test += "liga: " + match.Match.Liga + "\n";
                test += "link: " + match.Link + "\n";

                foreach (var matchTotal in match.Coefficient.BM)
                {
                    test += "key:" + matchTotal.Total + "\n";
                    test += "bk:" + matchTotal.BkName + "\n";
                    test += "more:" + matchTotal.More + "\n";
                    test += "less:" + matchTotal.More + "\n";
                }
                Console.WriteLine(test + "\n");
            }
        }
    }
}
