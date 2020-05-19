using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models
{
    public class CommandModels
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Голов
        /// </summary>
        public int Goal { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("Name={0}",Name)
                .AppendFormat(" : Goal={0}",Goal)
                .ToString();
        }
    }
}
