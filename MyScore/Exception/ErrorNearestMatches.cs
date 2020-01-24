using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreMatch.Exception
{
    [Serializable]
    public class ErrorNearestMatches : System.Exception
    {
        public ErrorNearestMatches(string Message) : base(Message) { }
    }
}
