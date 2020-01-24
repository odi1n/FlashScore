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

            foreach ( var pars in parsing )
            {
                Console.WriteLine(pars.Link + " | " + pars.DateStart);
            }
            Console.ReadKey();
        }
    }
}
