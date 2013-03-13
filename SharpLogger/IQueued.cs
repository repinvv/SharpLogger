using System;

namespace SharpLogger
{
    public delegate void ReceiveHandler<in T>(T item);

    public interface IQueued<T>
    {
        event ReceiveHandler<T> OnReceive;
        event Action OnTimeout;
        bool Active { get; }
        void SetTimeout(int value);
        void Send(T msg);
        void Terminate();
    }
}
