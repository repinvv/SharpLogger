using System;
using System.Threading;

namespace SharpLogger
{
    class PoolThreadStarter : IThreadStarter
    {
        Action<object> _action;

        public void Start(Action threadRun)
        {
            _action = x => threadRun();
            ThreadPool.QueueUserWorkItem(ActionStart);
        }

        public void Start(Action<object> threadRun, object argument)
        {
            _action = threadRun;
            ThreadPool.QueueUserWorkItem(ActionStart);
        }

        void ActionStart(object argument)
        {
            _action(argument);
        }

    }
}
