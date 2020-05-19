using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashScore.Models.H2H
{
    public class H2HTotalModels
    {
        /// <summary>
        /// Название команды которое получаем матчи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Матчи команды
        /// </summary>
        public List<H2HMatchModels> Match { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("Name={0}", Name)
                .AppendFormat(" : Match={0}", Match.Count)
                .ToString();
        }
    }
}
