using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreApi.Exception
{
    [Serializable]
    public class ErrorMatchesNull : System.Exception
    {
        public ErrorMatchesNull(string Message) : base(Message) { }
    }
}
