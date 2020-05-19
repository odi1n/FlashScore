using FlashScore.Models.Coefficient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models
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

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "Ceff")
                .AppendFormat(" : BM={0}", BM.Count)
                .AppendFormat(" : FDS={0}", FDS.Count)
                .ToString();
        }
    }
}
