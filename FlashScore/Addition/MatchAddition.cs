using FlashScore.Exception;
using FlashScore.Function;
using FlashScore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore
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
                Console.WriteLine($"INFO - Count={MatchesToday.Count} : Current={i} : {match.Match}");
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
                Console.WriteLine($"OVER-UNDER - Count={MatchesToday.Count} : Current={i} : {match.Coefficient}");
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
                Console.WriteLine($"H2H - Count={MatchesToday.Count} : Current={i} : {match.H2H.ToString()}");
            }
            return MatchesToday;
        }
    }
}
