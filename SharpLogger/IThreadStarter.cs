using System;

namespace SharpLogger
{
    interface IThreadStarter
    {
        void Start(Action threadRun);
        void Start(Action<object> threadRun, object argument);
    }
}
