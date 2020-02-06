using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyScoreApi.Models
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
        public Dictionary<double,List<TotalModels>> Bookmaker { get; set; }
    }
}
