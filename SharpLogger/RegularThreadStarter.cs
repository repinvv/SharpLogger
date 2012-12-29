using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace SharpLogger
{
    class RegularThreadStarter : IThreadStarter
    {
        Action<object> action;

        public void Start(Action threadRun)
        {
            action = (x) => threadRun();
            var thread = new Thread(ActionStart);
            thread.Start();
        }

        public void Start(Action<object> threadRun, object argument)
        {
            action = threadRun;
            var thread = new Thread(ActionStart);
            thread.Start(argument);
        }

        void ActionStart(object argument)
        {
            action(argument);
        }

    }
}
