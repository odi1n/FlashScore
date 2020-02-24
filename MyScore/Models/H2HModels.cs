using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class H2HModels
    {
        /// <summary>
        /// Последние ставки команда 1
        /// </summary>
        public InfoGameModels LastGameCommand1 { get; set; }
        /// <summary>
        /// Последние ставки команда 2
        /// </summary>
        public InfoGameModels LastGameCommand2 { get; set; }
        /// <summary>
        /// Очные ставки
        /// </summary>
        public InfoGameModels Confrontation { get; set; }
    }
}
