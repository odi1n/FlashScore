﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreApi.Exception
{
    [Serializable]
    public class ErrorMatchesNullException : System.Exception
    {
        public ErrorMatchesNullException(string Message) : base(Message) { }
    }
}
