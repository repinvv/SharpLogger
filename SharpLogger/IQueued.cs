using System;

namespace SharpLogger
{
    public delegate void ReceiveHandler<T>(T item);

    public interface IQueued<T>
    {
        event ReceiveHandler<T> OnReceive;
        event Action OnTimeout;
        bool Active { get; }
        void SetTimeout(int timeout);
        void Send(T msg);
        void Terminate();
    }
}
