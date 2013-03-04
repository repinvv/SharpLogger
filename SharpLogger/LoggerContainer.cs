using System.Collections.Generic;
using SharpLogger.Loggers;

namespace SharpLogger
{
    delegate IInternalLogger LogCreate(string category);

    class LoggerContainer
    {
        Dictionary<string, IInternalLogger> _loggers =
            new Dictionary<string, IInternalLogger>();
        LogCreate _logCreate;

        public LoggerContainer(LogCreate logCreate)
        {
            _logCreate = logCreate;
        }

        public string DefaultCategory { set; private get; }

        public IInternalLogger GetLogger(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                category = DefaultCategory;
            }
            IInternalLogger logger;
            lock (_loggers)
            {
                if (_loggers.TryGetValue(category, out logger))
                {
                    return logger;
                }
                logger = _logCreate(category);
                _loggers.Add(category, logger);
            }
            return logger;
        }

        public void SetSender(Sender send)
        {
            lock (_loggers)
            {
                foreach (var pair in _loggers)
                {
                    pair.Value.SetSender(send);
                }
            }
        }

        public void SetLevel(string category, int level)
        {
            lock (_loggers)
            {
                IInternalLogger logger;
                if (_loggers.TryGetValue(category, out logger))
                {
                    logger.SetLevel(level);
                }
            }
        }

        public void SetOneLevel(string category, int level, bool value)
        {
            lock (_loggers)
            {
                IInternalLogger logger;
                if (_loggers.TryGetValue(category, out logger))
                {
                    logger[level] = value;
                }
            }
        }

        public void SetLevelForAll(int level)
        {
            lock (_loggers)
            {
                foreach (var pair in _loggers)
                {
                    pair.Value.SetLevel(level);
                }
            }
        }

        public void SetOneLevelForAll(int level, bool value)
        {
            lock (_loggers)
            {
                foreach (var x in _loggers)
                {
                    x.Value[level] = value;
                }
            }
        }

    }
}
