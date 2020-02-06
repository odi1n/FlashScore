using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyScoreApi.Action;
using MyScoreApi;
using System.IO;

namespace MyScore_test
{
    class Program
    {
        static void Main(string[] args)
        {
            tesc();

            Console.ReadKey();
        }

        async static void  tesc()
        {
            MyScore myScore = new MyScore();

            var ss = await myScore.GetMatches(true);

            var infos = await ss.GetInfo();

            Console.Clear();

            string test = "";
            foreach ( var matches in infos )
            {
                test += ("name: " + matches.Name + "\n");
                test += ("time: " + matches.DateStart + "\n");
                test += ("liga: " + matches.Liga + "\n");
                test += ("link: " + matches.Link + "\n");
                foreach ( var match in matches.Bookmaker )
                {
                    test += ("key:" + match.Coef + "\n");
                    foreach ( var val in match.Total )
                    {
                        test += ("info: " + val.BkName + " | " + val.Less + " | " + val.More + "\n");
                    }
                }
                test += "\n";
            }

            Console.WriteLine(test);
            File.WriteAllText("test.txt", test);
        }
    }
}
