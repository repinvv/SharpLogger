using System;
using System.Collections.Generic;
using System.Threading;

namespace SharpLogger
{
    class LockQueued<T> : IQueued<T>
    {
        bool _active;
        int _timeout = Timeout.Infinite;
        Queue<T> _queue = new Queue<T>();
        object _queSync = new object();
        WaitHandle[] _events = new WaitHandle[2];
        private AutoResetEvent _sendEvent;

        public event ReceiveHandler<T> OnReceive;
        public event Action OnTimeout;

        public LockQueued(IThreadStarter starter, ManualResetEvent terminateEvent = null)
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
                lock (_queSync)
                {
                    _queue.Enqueue(msg);
                }
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
            int index;
            int currentTimeout = Timeout.Infinite;
            _active = true;
            while ((index = WaitHandle.WaitAny(_events, currentTimeout)) != 1)
            {
                if (index == WaitHandle.WaitTimeout)
                {
                    if (OnTimeout != null)
                    {
                        OnTimeout();
                        currentTimeout = Timeout.Infinite;
                    }
                }
                else
                {
                    if (_queue.Count > 0)
                    {
                        Queue<T> items = null;
                        bool keepDequeue = true;
                        while (keepDequeue)
                        {
                            lock (_queSync)
                            {
                                if (_queue.Count > 10)
                                {
                                    items = _queue;
                                    _queue = new Queue<T>();
                                    keepDequeue = false;

                                }
                                else
                                {
                                    T item = _queue.Dequeue();
                                    if (OnReceive != null)
                                    {
                                        OnReceive(item);
                                    }
                                    if (_queue.Count == 0)
                                    {
                                        keepDequeue = false;
                                    }
                                }
                            }
                        }
                        if (items != null)
                        {
                            foreach (T t in items)
                            {
                                if (OnReceive != null)
                                {
                                    OnReceive(t);
                                }
                            }
                        }
                    }
                    currentTimeout = _timeout;
                }

            }
            //terminated
            _active = false;
            lock (_queSync)
            {
                _queue.Clear();
            }
        }
    }
}
