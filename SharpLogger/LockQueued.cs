using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SharpLogger
{
    class LockQueued<T> : IQueued<T>
    {
        bool active = false;
        int timeout = Timeout.Infinite;
        Queue<T> queue = new Queue<T>();
        object queSync = new object();
        EventWaitHandle[] events = new EventWaitHandle[2];

        public event ReceiveHandler<T> OnReceive;
        public event Action OnTimeout;

        public LockQueued(IThreadStarter starter, ManualResetEvent terminateEvent = null)
        {
            events[0] = new AutoResetEvent(false);
            events[1] = terminateEvent ?? new ManualResetEvent(false);
            starter.Start(ThreadRun);
        }

        public bool Active
        {
            get { return active; }
        }

        public void SetTimeout(int timeout)
        {
            this.timeout = timeout;
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
            events[0].Set();
        }

        public void Terminate()
        {
            events[1].Set();
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
                    if (queue.Count != 0)
                    {
                        if (queue.Count == 1)
                        {
                            lock (queSync)
                            {
                                T item = queue.Dequeue();
                                if (OnReceive != null)
                                {
                                    OnReceive(item);
                                }
                            }
                        }
                        else
                        {
                            Queue<T> replacement = new Queue<T>();
                            Queue<T> items;
                            lock (queSync)
                            {
                                items = queue;
                                queue = replacement;
                            }
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
