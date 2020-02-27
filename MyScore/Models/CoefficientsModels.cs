using MyScore.Models.Coefficient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
{
    public class CoefficientsModels
    {
        /// <summary>
        /// Более менее
        /// </summary>
        public List<AllTotalModels> BM { get; set; }
        /// <summary>
        /// 1X2 - основное время
        /// </summary>
        public List<AllTotalModels> FDS { get; set; }
    }
}
