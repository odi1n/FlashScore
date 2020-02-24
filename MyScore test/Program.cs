using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyScore;
using System.IO;

namespace MyScore_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();

            Console.ReadKey();
        }

        async static void  Test()
        {
            MyScoreApi myScore = new MyScoreApi();
            var matches = await myScore.GetMatchesAsync(true);
            var info = await matches.GetInfoAsync();

            //Console.Clear();

            //foreach ( var match in info )
            //{
            //    string test = "";

            //    test += ("name: " + match.Name + "\n");
            //    test += ("time: " + match.DateStart + "\n");
            //    test += ("liga: " + match.Liga + "\n");
            //    test += ("link: " + match.Link + "\n");

            //    foreach ( var matchTotal in match.Bookmaker )
            //    {
            //        test += ("key:" + matchTotal.Coef + "\n");
            //        foreach ( var val in matchTotal.Total )
            //        {
            //            test += ("info: " + val.BkName + " | " + val.Less + " | " + val.More + "\n");
            //        }
            //    }
            //    Console.WriteLine(test + "\n");
            //}
        }
    }
}
