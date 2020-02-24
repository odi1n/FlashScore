using MyScore.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class MatchModels
    {
        /// <summary>
        /// Ссылка
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Id матча текущего
        /// </summary>
        public string MatchId {
            get
            {
                string id = Regex.Match(Link, "match/(.*?)/").Groups[1].Value;
                return id;
            }
        }
        /// <summary>
        /// Время начала матча
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Имя команд
        /// </summary>
        public string Name { get
            {
                return Command1.Name + " - " + Command2.Name;
            }
        }
        /// <summary>
        /// Команда 1
        /// </summary>
        public CommandModels Command1 { get; set; } = new CommandModels();
        /// <summary>
        /// Команда 2
        /// </summary>
        public CommandModels Command2 { get; set; } = new CommandModels();
        /// <summary>
        /// Лига
        /// </summary>
        public string Liga { get; set; }
        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Букмейкерская контора
        /// </summary>
        public List<AllTotalModels> Bookmaker { get; set; }
        /// <summary>
        /// Информация о голах команд
        /// </summary>
        public H2HModels H2H { get; set; }

        /// <summary>
        /// Получить информацию о матче полностью
        /// </summary>
        /// <param name="match">Матч</param>
        /// <returns></returns>
        public async Task<MatchModels> GetInfoAsync(bool info = true, bool overUnder = true,bool h2h = false)
        {
            MatchInfomation matchInfos = new MatchInfomation();
            MatchModels matchModels = this;

            if ( info )
            {
                var getMatchInfo = await matchInfos.GetMatchInfoAsync(this);
                matchModels.Command1 = getMatchInfo.Command1;
                matchModels.Command2 = getMatchInfo.Command2;
                matchModels.Country = getMatchInfo.Country;
                matchModels.Liga = getMatchInfo.Liga;
            }

            if ( overUnder )
            {
                var getOverUnder = await matchInfos.GetMatchOverUnderAsync(this);
                matchModels.Bookmaker = getOverUnder;
            }

            if ( h2h )
            {
                var getH2H = await matchInfos.GetH2H(this);
                matchModels.H2H = getH2H;
            }

            return matchModels;
        }

        /// <summary>
        /// Получить более менее команд
        /// </summary>
        /// <returns></returns>
        public async Task<MatchModels> GetOverUnderAsync()
        {
            MatchInfomation matchInfos = new MatchInfomation();
            MatchModels matchModels = this;

            var getOverUnder = await matchInfos.GetMatchOverUnderAsync(this);
            matchModels.Bookmaker = getOverUnder;

            return matchModels;
        }

        /// <summary>
        /// Получить голы команды
        /// </summary>
        /// <returns></returns>
        public async Task<MatchModels> GetH2HAsync()
        {
            MatchInfomation matchInfos = new MatchInfomation();
            MatchModels matchModels = this;

            var getH2H = await matchInfos.GetH2H(this);
            matchModels.H2H = getH2H;

            return matchModels;
        }
    }
}
