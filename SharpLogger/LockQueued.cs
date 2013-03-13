using System;
using System.Collections.Generic;
using System.Threading;

namespace SharpLogger
{
    class LockQueued<T> : IQueued<T>
    {
        bool active;
        int timeout = Timeout.Infinite;
        Queue<T> queue = new Queue<T>();
        object queSync = new object();
        WaitHandle[] events = new WaitHandle[2];
        private AutoResetEvent sendEvent;

        public event ReceiveHandler<T> OnReceive;
        public event Action OnTimeout;

        public LockQueued(IThreadStarter starter, ManualResetEvent terminateEvent = null)
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
                lock (queSync)
                {
                    queue.Enqueue(msg);
                }
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
            int index;
            int currentTimeout = Timeout.Infinite;
            active = true;
            while ((index = WaitHandle.WaitAny(events, currentTimeout)) != 1)
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
                    if (queue.Count > 0)
                    {
                        Queue<T> items = null;
                        bool keepDequeue = true;
                        while (keepDequeue)
                        {
                            lock (queSync)
                            {
                                if (queue.Count > 10)
                                {
                                    items = queue;
                                    queue = new Queue<T>();
                                    keepDequeue = false;

                                }
                                else
                                {
                                    T item = queue.Dequeue();
                                    if (OnReceive != null)
                                    {
                                        OnReceive(item);
                                    }
                                    if (queue.Count == 0)
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
                    currentTimeout = timeout;
                }

            }
            //terminated
            active = false;
            lock (queSync)
            {
                queue.Clear();
            }
        }
    }
}
