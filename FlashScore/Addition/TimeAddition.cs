using FlashScore.Exception;
using FlashScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore
{
    public static class TimeAddition
    {
        /// <summary>
        /// Получить матчи в течении указанных минут
        /// </summary>
        /// <param name="minutes">Минуты, матчи которые получим в ближайшие 60(по умолчанию) минут</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, int minutes = 60)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNullException("Список пуст, нужно получить значения");
            return MatchesToday.Where(x => x.Match.DateStart > DateTime.Now && x.Match.DateStart < DateTime.Now.AddMinutes(minutes)).ToList();
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
            x.Match.DateStart > DateTime.Now &&
            x.Match.DateStart < DateTime.Now.AddHours(nearestMatche.Hours).AddMinutes(nearestMatche.Minutes)).ToList();
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
            return MatchesToday.Where(x => x.Match.DateStart > start && x.Match.DateStart < end).ToList();
        }

        /// <summary>
        /// Получить все матчи до этого времени
        /// </summary>
        /// <param name="MatchesToday"></param>
        /// <param name="end">До которого времени получаем матчи</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, DateTime end)
        {
            return MatchesToday.Where(x => x.Match.DateStart > DateTime.Now && x.Match.DateStart < end).ToList();
        }
    }
}
