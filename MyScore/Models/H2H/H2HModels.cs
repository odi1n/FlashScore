using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models.H2H
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
    }
}
