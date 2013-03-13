using System;

namespace SharpLogger.Loggers
{
    delegate void Sender(LogItem message);

    class RegularLogger : IInternalLogger
    {
        Sender[] senders = new Sender[LogLevel.Total];
        string category;

        private void NullSend(LogItem message)
        {
        }

        public RegularLogger(string category, Sender sender, int level)
        {
            this.category = category;
            senders[0] = NullSend;
            senders[1] = sender ?? NullSend;
            SetLevel(level);
        }

        public void SetLevel(int level)
        {
            for (int n = LogLevel.Fatal; n < LogLevel.Total; n++)
            {
                if (n > level)
                    senders[n] = NullSend;
                else
                    senders[n] = senders[1];
            }
        }

        public void SetSender(Sender sender)
        {
            for (int n = 0; n < LogLevel.Total; n++)
            {
                if (senders[n] != NullSend)
                {
                    senders[n] = sender ?? NullSend;
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
                        senders[level] = senders[LogLevel.Always];
                    }
                    else
                    {
                        senders[level] = NullSend;
                    }

                }
            }
        }

        public void Always(string message)
        {
            senders[LogLevel.Always](new LogItem(category, LogLevel.Always, message));
        }

        public void Fatal(string message, Exception ex = null)
        {
            senders[LogLevel.Fatal](new LogItem(category, LogLevel.Fatal, message, ex: ex));
        }

        public void Error(string message, Exception ex = null)
        {
            senders[LogLevel.Error](new LogItem(category, LogLevel.Error, message, ex: ex));
        }

        public void Warning(string message, int id = 0, Exception ex = null)
        {
            senders[LogLevel.Warning](new LogItem(category, LogLevel.Warning, message, id, ex));
        }

        public void Warning(string message, int[] id, Exception ex = null)
        {
            senders[LogLevel.Warning](new LogItem(category, LogLevel.Warning, message, id, ex));
        }

        public void Warning(string message, Exception ex)
        {
            senders[LogLevel.Warning](new LogItem(category, LogLevel.Warning, message, ex: ex));
        }

        public void Info(string message, int id = 0)
        {
            senders[LogLevel.Info](new LogItem(category, LogLevel.Info, message, id));
        }

        public void Info(string message, int[] id)
        {
            senders[LogLevel.Info](new LogItem(category, LogLevel.Info, message, id));
        }

        public void Event(string message, int id = 0)
        {
            senders[LogLevel.Event](new LogItem(category, LogLevel.Event, message, id));
        }

        public void Event(string message, int[] id)
        {
            senders[LogLevel.Event](new LogItem(category, LogLevel.Event, message, id));
        }

        public void Debug(string message, int id = 0)
        {
            senders[LogLevel.Debug](new LogItem(category, LogLevel.Debug, message, id));
        }

        public void Debug(string message, int[] id)
        {
            senders[LogLevel.Debug](new LogItem(category, LogLevel.Debug, message, id));
        }

        public void Detailed(string message, int id = 0)
        {
            senders[LogLevel.All](new LogItem(category, LogLevel.All, message, id));
        }

        public void Detailed(string message, int[] id)
        {
            senders[LogLevel.All](new LogItem(category, LogLevel.All, message, id));
        }
    }
}
