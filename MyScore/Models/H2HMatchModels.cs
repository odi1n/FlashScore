using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScore.Models
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
    }
}
