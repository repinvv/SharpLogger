using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpLogger
{
    interface ILogWriter
    {
        void Write(LogItem message);
        void Flush();
        int GetTimeout();
    }
}
