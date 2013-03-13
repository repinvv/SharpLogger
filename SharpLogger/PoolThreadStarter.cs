using System;
using System.Threading;

namespace SharpLogger
{
    class PoolThreadStarter : IThreadStarter
    {
        Action<object> action;

        public void Start(Action threadRun)
        {
            action = x => threadRun();
            ThreadPool.QueueUserWorkItem(ActionStart);
        }

        public void Start(Action<object> threadRun, object argument)
        {
            action = threadRun;
            ThreadPool.QueueUserWorkItem(ActionStart);
        }

        void ActionStart(object argument)
        {
            action(argument);
        }

    }
}
