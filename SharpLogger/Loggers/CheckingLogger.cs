using System;

namespace SharpLogger.Loggers
{
    class CheckingLogger : IInternalLogger
    {
        bool[] _levels = new bool[LogLevel.Total];
        Sender _send;
        string _category;

        private void NullSend(LogItem message)
        {
        }

        public CheckingLogger(string category, Sender sender, int level)
        {
            _send = sender ?? NullSend;
            _category = category;
            SetLevel(level);
        }

        public void SetLevel(int level)
        {
            for (int n = LogLevel.Fatal; n < LogLevel.Total; n++)
            {
                _levels[n] = (n <= level);
            }
        }

        public void SetSender(Sender sender)
        {
            _send = sender ?? NullSend;
        }

        public bool this[int level]
        {
            set
            {
                if (level >= LogLevel.Fatal && level < LogLevel.Total)
                {
                    _levels[level] = value;
                }
            }
        }

        public void Always(string message)
        {
            _send(new LogItem(_category, LogLevel.Always, message));
        }

        public void Fatal(string message, Exception ex = null)
        {
            if (_levels[LogLevel.Fatal])
            {
                _send(new LogItem(_category, LogLevel.Fatal, message, ex: ex));
            }
        }

        public void Error(string message, Exception ex = null)
        {
            if (_levels[LogLevel.Error])
            {
                _send(new LogItem(_category, LogLevel.Error, message, ex: ex));
            }
        }

        public void Warning(string message, int id = 0, Exception ex = null)
        {
            if (_levels[LogLevel.Warning])
            {
                _send(new LogItem(_category, LogLevel.Warning, message, id, ex));
            }
        }

        public void Warning(string message, int[] id, Exception ex = null)
        {
            if (_levels[LogLevel.Warning])
            {
                _send(new LogItem(_category, LogLevel.Warning, message, id, ex));
            }
        }

        public void Warning(string message, Exception ex)
        {
            if (_levels[LogLevel.Warning])
            {
                _send(new LogItem(_category, LogLevel.Warning, message, ex: ex));
            }
        }

        public void Info(string message, int id = 0)
        {
            if (_levels[LogLevel.Info])
            {
                _send(new LogItem(_category, LogLevel.Info, message, id));
            }
        }

        public void Info(string message, int[] id)
        {
            if (_levels[LogLevel.Info])
            {
                _send(new LogItem(_category, LogLevel.Info, message, id));
            }
        }

        public void Event(string message, int id = 0)
        {
            if (_levels[LogLevel.Event])
            {
                _send(new LogItem(_category, LogLevel.Event, message, id));
            }
        }

        public void Event(string message, int[] id)
        {
            if (_levels[LogLevel.Event])
            {
                _send(new LogItem(_category, LogLevel.Event, message, id));
            }
        }

        public void Debug(string message, int id = 0)
        {
            if (_levels[LogLevel.Debug])
            {
                _send(new LogItem(_category, LogLevel.Debug, message, id));
            }
        }

        public void Debug(string message, int[] id)
        {
            if (_levels[LogLevel.Debug])
            {
                _send(new LogItem(_category, LogLevel.Debug, message, id));
            }
        }

        public void Detailed(string message, int id = 0)
        {
            if (_levels[LogLevel.All])
            {
                _send(new LogItem(_category, LogLevel.All, message, id));
            }
        }

        public void Detailed(string message, int[] id)
        {
            if (_levels[LogLevel.All])
            {
                _send(new LogItem(_category, LogLevel.All, message, id));
            }
        }
    }
}
