using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SharpLogger
{
    class ConcurrentQueued<T> : IQueued<T>
    {
        bool _active;
        int _timeout = Timeout.Infinite;
        ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        WaitHandle[] _events = new WaitHandle[2];
        private AutoResetEvent _sendEvent;

        public event ReceiveHandler<T> OnReceive;
        public event Action OnTimeout;

        public ConcurrentQueued(IThreadStarter starter, ManualResetEvent terminateEvent = null)
        {
            _events[0] = _sendEvent = new AutoResetEvent(false);
            _events[1] = terminateEvent ?? new ManualResetEvent(false);
            starter.Start(ThreadRun);
        }

        public bool Active
        {
            get { return _active; }
        }

        public void SetTimeout(int timeout)
        {
            _timeout = timeout;
        }

        public void Send(T msg)
        {
            if (!_active)
            {
                return;
            }
            try
            {
                _queue.Enqueue(msg);
            }
            catch (Exception)
            {
                //fail silently
            }
            _sendEvent.Set();
        }

        public void Terminate()
        {
            ((EventWaitHandle)_events[1]).Set();
        }

        void ThreadRun()
        {
            int currentTimeout = Timeout.Infinite;
            _active = true;
            try
            {
                int index;
                T item;
                while ((index = WaitHandle.WaitAny(_events, currentTimeout)) != 1)
                {
                    if (index == WaitHandle.WaitTimeout)
                    {
                        if (OnTimeout != null)
                        {
                            OnTimeout();
                        }
                        currentTimeout = Timeout.Infinite;
                    }
                    else
                    {
                        if (_queue.Count != 0)
                        {
                            while (_queue.TryDequeue(out item))
                            {
                                if (OnReceive != null)
                                {
                                    OnReceive(item);
                                }
                            }
                        }
                        currentTimeout = _timeout;
                    }

                }
                //terminated
                _active = false;
                while (_queue.TryDequeue(out item))
                {
                }
            }
            catch (Exception ex)
            {
                _timeout = ex.StackTrace.Length;
                //fail silently
            }

        }
    }
}
