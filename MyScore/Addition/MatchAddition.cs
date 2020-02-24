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
        /// <param name="match">Список выбранных матчей</param>
        /// <param name="info">Получить информацию о команде</param>
        /// <param name="overUnder">Получить более менее команды</param>
        /// <param name="h2h">Получить забитые голы H2H</param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetInfoAsync(this List<MatchModels> MatchesToday, bool info = true, bool overUnder = true,
            bool h2h = false)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();

            int i = 0;
            foreach ( var match in MatchesToday )
            {
                i++;

                if ( info )
                {
                    MatchModels getMatchInfo = await matchInfomathion.GetMatchInfoAsync(match);
                    match.Command1 = getMatchInfo.Command1;
                    match.Command2 = getMatchInfo.Command2;
                    match.Country = getMatchInfo.Country;
                    match.Liga = getMatchInfo.Liga;
                }
                if ( overUnder )
                {
                    List<AllTotalModels>  getOverUnder = await matchInfomathion.GetMatchOverUnderAsync(match);
                    match.Bookmaker = getOverUnder;
                }
                if ( h2h )
                {
                    H2HModels getH2H = await matchInfomathion.GetH2H(match);
                    match.H2H = getH2H;
                }

                Console.WriteLine($"INFO - count={MatchesToday.Count}, current={i}, lige={match.Liga} match=\"{match.Name}\", start = {match.DateStart.Value.ToString("f", CultureInfo.GetCultureInfo("ru-ru"))}");
            }
            return MatchesToday;
        }

        /// <summary>
        /// Получить более/менее команды
        /// </summary>
        /// <param name="MatchesToday"></param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetOverUnder(this List<MatchModels> MatchesToday)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();
            int i = 0;
            foreach ( var match in MatchesToday )
            {
                match.Bookmaker = await matchInfomathion.GetMatchOverUnderAsync(match);

                Console.WriteLine($"OVER-UNDER - count={MatchesToday.Count}, current={i}, bool={match.Bookmaker.Count}");
                i++;
            }
            return MatchesToday;
        }

        /// <summary>
        /// Получить информацию о голах
        /// </summary>
        /// <param name="MatchesToday"></param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetH2H(this List<MatchModels> MatchesToday)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();
            int i = 0;
            foreach ( var match in MatchesToday )
            {
                match.H2H = await matchInfomathion.GetH2H(match);
                Console.WriteLine($"H2H - count={MatchesToday.Count}, current={i}, name={match.H2H.Confrontation.Name}, comm1 = {match.H2H.LastGameCommand1.Match.Count}, comm2 = {match.H2H.LastGameCommand2.Match.Count}, conf = {match.H2H.Confrontation.Match.Count}");
                i++;
            }
            return MatchesToday;
        }

        /// <summary>
        /// Получить матчи в течении указанных минут
        /// </summary>
        /// <param name="minutes">Минуты, матчи которые получим в ближайшие 60(по умолчанию) минут</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, int minutes = 60)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNullException("Список пуст, нужно получить значения");
            return MatchesToday.Where(x => x.DateStart > DateTime.Now && x.DateStart < DateTime.Now.AddMinutes(minutes)).ToList();
        }

        /// <summary>
        /// Получить матчи в указанном промежутке
        /// </summary>
        /// <param name="nearestMatche">Указать часы или минуты</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, NearestMatchesModels nearestMatche)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNullException("Список пуст, нужно получить значения");
            if ( nearestMatche.Hours > 24 || nearestMatche.Hours < -24 ||
                nearestMatche.Minutes > 1440 || nearestMatche.Minutes < -1440 )
                throw new ErrorNearestMatchesException("Указано времени больше чем может быть в сутках");

            return MatchesToday.Where(x =>
            x.DateStart > DateTime.Now &&
            x.DateStart < DateTime.Now.AddHours(nearestMatche.Hours).AddMinutes(nearestMatche.Minutes)).ToList();
        }

        /// <summary>
        /// Получить матчи только в определенном промежутке
        /// </summary>
        /// <param name="MatchesToday"></param>
        /// <param name="start">От скольки</param>
        /// <param name="end">До скольки</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime start, DateTime end)
        {
            return MatchesToday.Where(x => x.DateStart > start && x.DateStart < end).ToList();
        }

        /// <summary>
        /// Получить все матчи до этого времени
        /// </summary>
        /// <param name="MatchesToday"></param>
        /// <param name="end">До которого времени получаем матчи</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime end)
        {
            return MatchesToday.Where(x => x.DateStart > DateTime.Now && x.DateStart < end).ToList();
        }
    }
}
