using System;
using System.Threading;
namespace SharpLogger
{
    class RegularThreadStarter : IThreadStarter
    {
        Action<object> _action;

        public void Start(Action threadRun)
        {
            _action = x => threadRun();
            var thread = new Thread(ActionStart);
            thread.Start();
        }

        public void Start(Action<object> threadRun, object argument)
        {
            _action = threadRun;
            var thread = new Thread(ActionStart);
            thread.Start(argument);
        }

        void ActionStart(object argument)
        {
            _action(argument);
        }

    }
}
