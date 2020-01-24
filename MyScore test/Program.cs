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


            var infoOverUnder = query.GetInfo(query.MatchesToday[5]);

            Console.Clear();

            Console.WriteLine("name: " +infoOverUnder.Name);
            foreach ( var match in infoOverUnder.Bookmaker )
            {
                Console.WriteLine("key:"+match.Key  );
                foreach(var val in match.Value )
                {
                    Console.WriteLine("info: " + val.BkName + " | " + val.Less + " | " + val.More );
                }
            }



            Console.ReadKey();
        }
    }
}
