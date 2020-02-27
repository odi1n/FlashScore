using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class MatchInfoModels
    {
        /// <summary>
        /// Имя команд
        /// </summary>
        public string Name
        {
            get
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
        /// Время начала матча
        /// </summary>
        public DateTime? DateStart { get; set; }
    }
}
