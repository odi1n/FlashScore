using FlashScore.Action;
using FlashScore.Enums;
using FlashScore.Function;
using FlashScore.Models.Coefficient;
using FlashScore.Models.H2H;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlashScore.Models
{
    public class MatchModels
    {
        /// <summary>
        /// Ссылка
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Id текущего матча
        /// </summary>
        public string Id {get{return Regex.Match(Link, "match/(.*?)/").Groups[1].Value;} }
        /// <summary>
        /// Формула
        /// </summary>
        public double Formula { get; set; }
        /// <summary>
        /// Результаты матча
        /// </summary>
        public ResultMatch Result { get; set; } = ResultMatch.None;
        /// <summary>
        /// Информация о матче
        /// </summary>
        public MatchInfoModels Match { get; set; } = new MatchInfoModels();
        /// <summary>
        /// Вкладка коэфициенты
        /// </summary>
        public CoefficientsModels Coefficient { get; set; }
        /// <summary>
        /// Информация о голах команд
        /// </summary>
        public H2HModels H2H { get; set; }

        /// <summary>
        /// Получить всю информацию
        /// </summary>
        /// <param name="match">Матч</param>
        /// <returns></returns>
        public async Task<MatchModels> GetAllInfoAsync(bool info = true, bool fds = true, bool bm = true, bool h2h = false)
        {
            if ( info )
                await GetMatchInfoAsync();

            if ( fds || bm )
                await GetPageCoefficient(fds, bm);

            if ( h2h )
                await GetH2HAsync();

            return this;
        }

        /// <summary>
        /// Получить информацию о матче
        /// </summary>
        /// <returns></returns>
        public async Task<MatchModels> GetMatchInfoAsync()
        {
            var getMatchInfo = await new MatchInfomation().GetMatchInfoAsync(this);

            if ( getMatchInfo.Match.DateStart == null )
                getMatchInfo.Match.DateStart = this.Match.DateStart;

            this.Match = getMatchInfo.Match;
            this.Result = getMatchInfo.Result;

            return this;
        }

        /// <summary>
        /// Получить более менее команд
        /// </summary>
        /// <returns></returns>
        public async Task<MatchModels> GetPageCoefficient(bool fds = true, bool bm = true)
        {
            List<AllTotalModels> coeffBM = null, coeffFDS = null;

            var coeffPage = await new MatchInfomation().GetPageCoefficient(this);
           
            if (fds )
                coeffFDS = Parsing.CoeffFDS(coeffPage); 
            if ( bm )
                coeffBM = Parsing.CoeffBM(coeffPage);

            this.Coefficient = new CoefficientsModels()
            {
                BM = coeffBM,
                FDS = coeffFDS,
            };

            return this;
        }

        /// <summary>
        /// Получить голы команды
        /// </summary>
        /// <returns></returns>
        public async Task<MatchModels> GetH2HAsync()
        {
            var getH2H = await new MatchInfomation().GetH2H(this);

            this.H2H = getH2H;

            return this;
        }
    }
}
