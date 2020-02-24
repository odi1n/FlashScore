using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class AllTotalModels
    {
        /// <summary>
        /// Тотал команды
        /// </summary>
        public double Total { get; set; }
        /// <summary>
        /// Больше меньше
        /// </summary>
        public List<TotalModels> Info { get; set; }
    }
}
