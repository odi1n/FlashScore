﻿using MyScoreApi.Exception;
using MyScoreApi.Function;
using MyScoreApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreApi
{
   
    public static class MatchAddition
    {
        /// <summary>
        /// Получить информацию обо всех матчах которые были выбраны
        /// </summary>
        /// <param name="match">Список выбранных матчей</param>
        /// <returns></returns>
        public static async Task<List<MatchModels>> GetInfo(this List<MatchModels> MatchesToday)
        {
            MatchInfomation matchInfomathion = new MatchInfomation();

            int i = 0;
            foreach ( var match in MatchesToday )
            {
                i++;
                var matchInfo = await matchInfomathion.GetMatchInfo(match);
                var overUnder = await matchInfomathion.GetMatchOverUnder(match);

                match.Command1 = matchInfo.Command1;
                match.Command2 = matchInfo.Command2;
                match.Country = matchInfo.Country;
                match.Liga = matchInfo.Liga;

                if ( matchInfo.DateStart == null ) match.DateStart = matchInfo.DateStart;

                match.Bookmaker = overUnder;
                Console.WriteLine(i + ":" + MatchesToday.Count);
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
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            return MatchesToday.Where(x => x.DateStart > DateTime.Now && x.DateStart < DateTime.Now.AddMinutes(minutes)).ToList();
        }

        /// <summary>
        /// Получить матчи в течении указанных часов
        /// </summary>
        /// <param name="hours">Часы, матчи которые получим в ближайшие 1(по умолчанию) час</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, long hours = 1)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            return MatchesToday.Where(x =>
            x.DateStart > DateTime.Now &&
            x.DateStart < DateTime.Now.AddHours(hours)).ToList();
        }

        /// <summary>
        /// Получить матчи в указанном промежутке
        /// </summary>
        /// <param name="nearestMatche">Указать часы или минуты</param>
        /// <returns></returns>
        public static List<MatchModels> GetNearest(this List<MatchModels> MatchesToday, NearestMatchesModels nearestMatche)
        {
            if ( MatchesToday.Count == 0 ) throw new ErrorMatchesNull("Список пуст, нужно получить значения");
            if ( nearestMatche.Hours > 24 || nearestMatche.Hours < -24 ||
                nearestMatche.Minutes > 1440 || nearestMatche.Minutes < -1440 )
                throw new ErrorNearestMatches("Указано времени больше чем может быть в сутках");

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
