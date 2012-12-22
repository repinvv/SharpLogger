using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;
namespace SharpLogger
{
    delegate InternalLogger LogCreate(string category);

    class LoggerContainer
    {
        IOptions options;
        Dictionary<string, InternalLogger> loggers =
            new Dictionary<string, InternalLogger>();
        LogCreate logCreate;

        public LoggerContainer(IOptions options, LogCreate logCreate)
        {
            this.options = options;
            this.logCreate = logCreate;
        }

        public InternalLogger GetLogger(string category)
        {
            if (category == null || category == string.Empty)
            {
                options.Get("LogDefaultCategory", "Default");
            }
            InternalLogger logger = null;
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
            InternalLogger logger;
            lock (loggers)
            {
                if (loggers.TryGetValue(category, out logger))
                {
                    logger.SetLevel(level);
                }
            }
        }

        public void SetOneLevel(string category, int level, bool value)
        {
            InternalLogger logger;
            lock (loggers)
            {
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
