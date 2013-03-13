using System.Collections.Generic;
using SharpLogger.Loggers;

namespace SharpLogger
{
    delegate IInternalLogger LogCreate(string category);

    class LoggerContainer
    {
        Dictionary<string, IInternalLogger> loggers =
            new Dictionary<string, IInternalLogger>();
        LogCreate logCreate;

        public LoggerContainer(LogCreate logCreate)
        {
            this.logCreate = logCreate;
        }

        public string DefaultCategory { set; private get; }

        public IInternalLogger GetLogger(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                category = DefaultCategory;
            }
            IInternalLogger logger;
            lock (loggers)
            {
                if (loggers.TryGetValue(category, out logger))
                {
                    return logger;
                }
                logger = logCreate(category);
                loggers.Add(category, logger);
            }
            return logger;
        }

        public void SetSender(Sender send)
        {
            lock (loggers)
            {
                foreach (var pair in loggers)
                {
                    pair.Value.SetSender(send);
                }
            }
        }

        public void SetLevel(string category, int level)
        {
            lock (loggers)
            {
                IInternalLogger logger;
                if (loggers.TryGetValue(category, out logger))
                {
                    logger.SetLevel(level);
                }
            }
        }

        public void SetOneLevel(string category, int level, bool value)
        {
            lock (loggers)
            {
                IInternalLogger logger;
                if (loggers.TryGetValue(category, out logger))
                {
                    logger[level] = value;
                }
            }
        }

        public void SetLevelForAll(int level)
        {
            lock (loggers)
            {
                foreach (var pair in loggers)
                {
                    pair.Value.SetLevel(level);
                }
            }
        }

        public void SetOneLevelForAll(int level, bool value)
        {
            lock (loggers)
            {
                foreach (var x in loggers)
                {
                    x.Value[level] = value;
                }
            }
        }

    }
}
