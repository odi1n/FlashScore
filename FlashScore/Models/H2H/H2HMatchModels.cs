using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models.H2H
{
    public class H2HMatchModels
    {
        /// <summary>
        /// Информация о первой команде
        /// </summary>
        public CommandModels Command1 { get; set; }
        /// <summary>
        /// Информация о второй команде
        /// </summary>
        public CommandModels Command2 { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat(" : Command1='{0}'", Command1)
                .AppendFormat(" : Command2='{0}'", Command2)
                .ToString();
        }
    }
}
