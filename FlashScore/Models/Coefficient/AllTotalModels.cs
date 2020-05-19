using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models.Coefficient
{
    public class AllTotalModels
    {
        /// <summary>
        /// Название БК конторы
        /// </summary>
        public string BkName { get; set; }
        /// <summary>
        /// Тотал команды
        /// </summary>
        public double? Total { get; set; }
        /// <summary>
        /// Больше
        /// </summary>
        public double? More { get; set; }
        /// <summary>
        /// Меньше
        /// </summary>
        public double? Less { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public double? First { get; set; }
        /// <summary>
        /// X
        /// </summary>
        public double? X { get; set; }
        /// <summary>
        /// 2
        /// </summary>
        public double? Two { get; set; }

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", BkName)
                .AppendFormat(" : Total={0}", Total)
                .AppendFormat(" : More={0}", More)
                .AppendFormat(" : Less={0}", Less)
                .AppendFormat(" : First={0}", First)
                .AppendFormat(" : X={0}", X)
                .AppendFormat(" : Two={0}", Two)
                .ToString();
        }
    }
}
