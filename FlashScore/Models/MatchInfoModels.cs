using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models
{
    public class MatchInfoModels
    {
        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Лига
        /// </summary>
        public string Liga { get; set; }
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
        /// Время начала матча
        /// </summary>
        public DateTime? DateStart { get; set; }
        /// <summary>
        /// Команда 1
        /// </summary>
        public CommandModels Command1 { get; set; } = new CommandModels();
        /// <summary>
        /// Команда 2
        /// </summary>
        public CommandModels Command2 { get; set; } = new CommandModels();

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "Match")
                .AppendFormat(" : Country={0}", Country)
                .AppendFormat(" : Liga={0}", Liga)
                .AppendFormat(" : Match={0}", Name)
                .AppendFormat(" : Date={0}", DateStart.Value.ToString("f", CultureInfo.GetCultureInfo("ru-ru")))
                .AppendFormat(" : Command1='{0}'", Command1)
                .AppendFormat(" : Command2='{0}'", Command2)
                .ToString();
        }
    }
}
