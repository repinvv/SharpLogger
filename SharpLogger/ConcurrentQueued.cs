using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SharpLogger
{
    class ConcurrentQueued<T> : IQueued<T>
    {
        bool active;
        int timeout = Timeout.Infinite;
        ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
        WaitHandle[] events = new WaitHandle[2];
        private AutoResetEvent sendEvent;

        public event ReceiveHandler<T> OnReceive;
        public event Action OnTimeout;

        public ConcurrentQueued(IThreadStarter starter, ManualResetEvent terminateEvent = null)
        {
            events[0] = sendEvent = new AutoResetEvent(false);
            events[1] = terminateEvent ?? new ManualResetEvent(false);
            starter.Start(ThreadRun);
        }

        public bool Active
        {
            get { return active; }
        }

        public void SetTimeout(int value)
        {
            timeout = value;
        }

        public void Send(T msg)
        {
            if (!active)
            {
                return;
            }
            try
            {
                queue.Enqueue(msg);
            }
            catch (Exception)
            {
                //fail silently
            }
            sendEvent.Set();
        }

        public void Terminate()
        {
            ((EventWaitHandle)events[1]).Set();
        }

        void ThreadRun()
        {
            int currentTimeout = Timeout.Infinite;
            active = true;
            try
            {
                int index;
                T item;
                while ((index = WaitHandle.WaitAny(events, currentTimeout)) != 1)
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
                        if (queue.Count != 0)
                        {
                            while (queue.TryDequeue(out item))
                            {
                                if (OnReceive != null)
                                {
                                    OnReceive(item);
                                }
                            }
                        }
                        currentTimeout = timeout;
                    }

                }
                //terminated
                active = false;
                while (queue.TryDequeue(out item))
                {
                }
            }
            catch (Exception ex)
            {
                timeout = ex.StackTrace.Length;
                //fail silently
            }

        }
    }
}
