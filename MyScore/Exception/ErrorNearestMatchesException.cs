﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreApi.Exception
{
    [Serializable]
    public class ErrorNearestMatchesException : System.Exception
    {
        public ErrorNearestMatchesException(string Message) : base(Message) { }
    }
}
