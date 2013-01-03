using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SharpLogger
{
    class NotQueued : IQueued<LogItem>
    {
        public event ReceiveHandler<LogItem> OnReceive;

        public event Action OnTimeout;

        Timer timer = new Timer(500);

        public NotQueued()
        {
            timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (timer)
            {
                OnTimeout();
                timer.Stop();
            }
        }

        public bool Active
        {
            get { return true; }
        }

        public void SetTimeout(int timeout)
        {
            timer.Interval = timeout;
        }

        public void Send(LogItem msg)
        {
            lock (timer)
            {
                OnReceive(msg);
                timer.Stop();
                timer.Start();
            }
        }

        public void Terminate()
        {         
        }
    }
}
