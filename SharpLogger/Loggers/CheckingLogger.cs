using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class CheckingLogger : InternalLogger
    {
        bool[] levels = new bool[LogLevel.Total];
        Sender send;
        string category;

        private void NullSend(LogItem message)
        {
        }

        public CheckingLogger(string category, Sender sender, int level)
        {
            send = sender ?? NullSend;
            this.category = category;
            SetLevel(level);
        }

        public void SetLevel(int level)
        {
            for (int n = LogLevel.Fatal; n < LogLevel.Total; n++)
            {
                levels[n] = (n <= level);
            }
        }

        public void SetSender(Sender sender)
        {
            send = sender ?? NullSend;
        }

        public bool this[int level]
        {
            set
            {
                if (level >= LogLevel.Fatal && level < LogLevel.Total)
                {
                    levels[level] = value;
                }
            }
        }

        public void Always(string message)
        {
            send(new LogItem(category, LogLevel.Always, message));
        }

        public void Fatal(string message, Exception ex = null)
        {
            if (levels[LogLevel.Fatal])
            {
                send(new LogItem(category, LogLevel.Fatal, message, ex: ex));
            }
        }

        public void Error(string message, Exception ex = null)
        {
            if (levels[LogLevel.Error])
            {
                send(new LogItem(category, LogLevel.Error, message, ex: ex));
            }
        }

        public void Warning(string message, int id = 0, Exception ex = null)
        {
            if (levels[LogLevel.Warning])
            {
                send(new LogItem(category, LogLevel.Warning, message, id, ex));
            }
        }

        public void Warning(string message, int[] id, Exception ex = null)
        {
            if (levels[LogLevel.Warning])
            {
                send(new LogItem(category, LogLevel.Warning, message, id, ex));
            }
        }

        public void Warning(string message, Exception ex)
        {
            if (levels[LogLevel.Warning])
            {
                send(new LogItem(category, LogLevel.Warning, message, ex: ex));
            }
        }

        public void Info(string message, int id = 0)
        {
            if (levels[LogLevel.Info])
            {
                send(new LogItem(category, LogLevel.Info, message, id));
            }
        }

        public void Info(string message, int[] id)
        {
            if (levels[LogLevel.Info])
            {
                send(new LogItem(category, LogLevel.Info, message, id));
            }
        }

        public void Event(string message, int id = 0)
        {
            if (levels[LogLevel.Event])
            {
                send(new LogItem(category, LogLevel.Event, message, id));
            }
        }

        public void Event(string message, int[] id)
        {
            if (levels[LogLevel.Event])
            {
                send(new LogItem(category, LogLevel.Event, message, id));
            }
        }

        public void Debug(string message, int id = 0)
        {
            if (levels[LogLevel.Debug])
            {
                send(new LogItem(category, LogLevel.Debug, message, id));
            }
        }

        public void Debug(string message, int[] id)
        {
            if (levels[LogLevel.Debug])
            {
                send(new LogItem(category, LogLevel.Debug, message, id));
            }
        }

        public void Detailed(string message, int id = 0)
        {
            if (levels[LogLevel.All])
            {
                send(new LogItem(category, LogLevel.All, message, id));
            }
        }

        public void Detailed(string message, int[] id)
        {
            if (levels[LogLevel.All])
            {
                send(new LogItem(category, LogLevel.All, message, id));
            }
        }
    }
}
