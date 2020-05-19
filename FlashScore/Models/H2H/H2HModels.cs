using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models.H2H
{
    public class H2HModels
    {
        /// <summary>
        /// Последние ставки команда 1
        /// </summary>
        public H2HTotalModels LastGameCommand1 { get; set; }
        /// <summary>
        /// Последние ставки команда 2
        /// </summary>
        public H2HTotalModels LastGameCommand2 { get; set; }
        /// <summary>
        /// Очные ставки
        /// </summary>
        public H2HTotalModels Confrontation { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "H2H")
                .AppendFormat(" : LastGameCommand1='{0}'", LastGameCommand1)
                .AppendFormat(" : LastGameCommand2='{0}'", LastGameCommand2)
                .AppendFormat(" : Confrontation='{0}'", Confrontation)
                .ToString();
        }
    }
}
