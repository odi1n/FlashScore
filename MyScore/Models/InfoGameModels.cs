using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class InfoGameModels
    {
        /// <summary>
        /// Название команды которое получаем матчи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Матчи команды
        /// </summary>
        public List<H2HMatchModels> Match { get; set; }
    }
}
