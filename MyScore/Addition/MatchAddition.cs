using MyScore.Exception;
using MyScore.Function;
using MyScore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore
{
    public static class MatchAddition
    {
        /// <summary>
        /// Получить информацию обо всех матчах которые были выбраны
        /// </summary>
        /// <param name="MatchesToday">Список матчей</param>
        /// <param name="info">Получить информацию о команде</param>
        /// <param name="fds">1х2 - основное время</param>
        /// <param name="bm">Больше меньше</param>
        /// <param name="h2h">Получить забитые голы H2H</param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetInfoAsync(this List<MatchModels> MatchesToday, bool info = true, bool fds = true, bool bm = true,
            bool h2h = false)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();

            int i = 0;
            foreach ( var match in MatchesToday )
            {
                i++;
                await match.GetAllInfoAsync(info,fds,bm, h2h);
                Console.WriteLine($"INFO - count={MatchesToday.Count}, current={i}, lige={match.Match.Liga} match=\"{match.Match.Name}\", start = {match.Match.DateStart.Value.ToString("f", CultureInfo.GetCultureInfo("ru-ru"))}");
            }
            return MatchesToday;
        }

        /// <summary>
        /// Получить информацию с вкладки коэфициент
        /// </summary>
        /// <param name="MatchesToday">Список матчей</param>
        /// <param name="fds">1х2 - основное время</param>
        /// <param name="bm">Больше меньше</param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetCoefficient(this List<MatchModels> MatchesToday, bool fds = true, bool bm = true)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();
            int i = 0;
            foreach ( var match in MatchesToday )
            {
                i++;
                await match.GetPageCoefficient();
                Console.WriteLine($"OVER-UNDER - count={MatchesToday.Count}, current={i}, bool={match.Coefficient.BM.Count}");
            }
            return MatchesToday;
        }

        /// <summary>
        /// Получить информацию о забитых голах команд
        /// </summary>
        /// <param name="MatchesToday"></param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetH2H(this List<MatchModels> MatchesToday)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();
            int i = 0;
            foreach ( var match in MatchesToday )
            {
                i++;
                await match.GetH2HAsync();
                Console.WriteLine($"H2H - count={MatchesToday.Count}, current={i}, name={match.H2H.Confrontation.Name}, comm1 = {match.H2H.LastGameCommand1.Match.Count}, comm2 = {match.H2H.LastGameCommand2.Match.Count}, conf = {match.H2H.Confrontation.Match.Count}");
            }
            return MatchesToday;
        }
    }
}
