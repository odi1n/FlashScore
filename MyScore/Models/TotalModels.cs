using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class TotalModels
    {
        /// <summary>
        /// Название БК конторы
        /// </summary>
        public string BkName { get; set; }
        /// <summary>
        /// Больше
        /// </summary>
        public double? More { get; set; }
        /// <summary>
        /// Меньше
        /// </summary>
        public double? Less { get; set; }
    }
}
