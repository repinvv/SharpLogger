using System;

namespace SharpLogger.Loggers
{
    delegate void Sender(LogItem message);

    class RegularLogger : IInternalLogger
    {
        Sender[] _senders = new Sender[LogLevel.Total];
        string _category;

        private void NullSend(LogItem message)
        {
        }

        public RegularLogger(string category, Sender sender, int level)
        {
            _category = category;
            _senders[0] = NullSend;
            _senders[1] = sender ?? NullSend;
            SetLevel(level);
        }

        public void SetLevel(int level)
        {
            for (int n = LogLevel.Fatal; n < LogLevel.Total; n++)
            {
                if (n > level)
                    _senders[n] = NullSend;
                else
                    _senders[n] = _senders[1];
            }
        }

        public void SetSender(Sender sender)
        {
            for (int n = 0; n < LogLevel.Total; n++)
            {
                if (_senders[n] != NullSend)
                {
                    _senders[n] = sender ?? NullSend;
                }
            }
        }

        public bool this[int level]
        {
            set
            {
                if (level >= LogLevel.Fatal && level < LogLevel.Total)
                {
                    if (value)
                    {
                        _senders[level] = _senders[LogLevel.Always];
                    }
                    else
                    {
                        _senders[level] = NullSend;
                    }

                }
            }
        }

        public void Always(string message)
        {
            _senders[LogLevel.Always](new LogItem(_category, LogLevel.Always, message));
        }

        public void Fatal(string message, Exception ex = null)
        {
            _senders[LogLevel.Fatal](new LogItem(_category, LogLevel.Fatal, message, ex: ex));
        }

        public void Error(string message, Exception ex = null)
        {
            _senders[LogLevel.Error](new LogItem(_category, LogLevel.Error, message, ex: ex));
        }

        public void Warning(string message, int id = 0, Exception ex = null)
        {
            _senders[LogLevel.Warning](new LogItem(_category, LogLevel.Warning, message, id, ex));
        }

        public void Warning(string message, int[] id, Exception ex = null)
        {
            _senders[LogLevel.Warning](new LogItem(_category, LogLevel.Warning, message, id, ex));
        }

        public void Warning(string message, Exception ex)
        {
            _senders[LogLevel.Warning](new LogItem(_category, LogLevel.Warning, message, ex: ex));
        }

        public void Info(string message, int id = 0)
        {
            _senders[LogLevel.Info](new LogItem(_category, LogLevel.Info, message, id));
        }

        public void Info(string message, int[] id)
        {
            _senders[LogLevel.Info](new LogItem(_category, LogLevel.Info, message, id));
        }

        public void Event(string message, int id = 0)
        {
            _senders[LogLevel.Event](new LogItem(_category, LogLevel.Event, message, id));
        }

        public void Event(string message, int[] id)
        {
            _senders[LogLevel.Event](new LogItem(_category, LogLevel.Event, message, id));
        }

        public void Debug(string message, int id = 0)
        {
            _senders[LogLevel.Debug](new LogItem(_category, LogLevel.Debug, message, id));
        }

        public void Debug(string message, int[] id)
        {
            _senders[LogLevel.Debug](new LogItem(_category, LogLevel.Debug, message, id));
        }

        public void Detailed(string message, int id = 0)
        {
            _senders[LogLevel.All](new LogItem(_category, LogLevel.All, message, id));
        }

        public void Detailed(string message, int[] id)
        {
            _senders[LogLevel.All](new LogItem(_category, LogLevel.All, message, id));
        }
    }
}
