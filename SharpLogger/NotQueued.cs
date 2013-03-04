using System;
using System.Timers;

namespace SharpLogger
{
    class NotQueued : IQueued<LogItem>
    {
        public event ReceiveHandler<LogItem> OnReceive;

        public event Action OnTimeout;

        Timer _timer = new Timer(500);

        public NotQueued()
        {
            _timer.Elapsed += OnTimedEvent;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (_timer)
            {
                OnTimeout();
                _timer.Stop();
            }
        }

        public bool Active
        {
            get { return true; }
        }

        public void SetTimeout(int timeout)
        {
            _timer.Interval = timeout;
        }

        public void Send(LogItem msg)
        {
            lock (_timer)
            {
                OnReceive(msg);
                _timer.Stop();
                _timer.Start();
            }
        }

        public void Terminate()
        {         
        }
    }
}
