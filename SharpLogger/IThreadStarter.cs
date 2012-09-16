using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpLogger
{
   interface IThreadStarter
   {
      void Start(Action threadRun);
      void Start(Action<object> threadRun, object argument);
   }
}
