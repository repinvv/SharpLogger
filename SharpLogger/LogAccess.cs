using SharpLogger.LogWrite;
using SharpLogger.Loggers;
using SharpOptions;

namespace SharpLogger
{
    //entry point static class for the logger
    public static class LogAccess
    {
        static bool _init;
        static object _sync = new object();
        static ILogger _nullLogger = new NullLogger();
        static LogCollector _collector;
        static LoggerContainer _container;
        static IOptions _options;

        static LogAccess()
        {
        }

        /// <summary>
        /// Initiates logging.
        /// </summary>
        static public void Init()
        {
            lock (_sync)
            {
                _options = null;
                InitBody();
            }
        }

        /// Initiates logger with options.
        /// Used options keys are:
        /// ---text output---
        /// LogFormat - see instruction
        /// ---levels---
        ///LogLevel - level at start
        ///LogAlwaysString
        ///LogFatalString
        ///LogErrorString
        ///LogWarningString
        ///LogInfoString
        ///LogEventString
        ///LogDebugString
        ///LogAllString
        /// ---category---
        ///LogDefaultCategory
        /// ---time format---
        ///LogDateTimeFormat
        ///LogMilisecondsFormat
        /// ---Id formatting---
        ///LogIDSplitChar
        ///LogIDBracketLeft
        ///LogIDBracketRight
        ///---Writer selector---
        ///LogWriteTarget - can be File or Database
        /// ---File writer options---
        ///LogFilename
        ///LogFileFlush
        ///LogFileAppend
        ///LogRotationSizeMb
        ///LogRotationFiles
        ///---Database writer options---
        ///LogConnectionString
        ///LogWriteTable
        ///LogDataFlush
        ///LogDaysToKeep
        static public void Init(IOptions options)
        {
            lock (_sync)
            {
                _options = options;
                InitBody();
            }
        }

        static void InitBody()
        {
            if (_options == null)
            {
                _options = new Options("", "Log");
            }
            LogDefaultOptions.AddDefaultOptions(_options);
            LogLevel.SetLevel(LogLevel.Always, _options[LogDefaultOptions.AlwaysOption]);
            LogLevel.SetLevel(LogLevel.Fatal,_options[LogDefaultOptions.FatalOption]);
            LogLevel.SetLevel(LogLevel.Error,_options[LogDefaultOptions.ErrorOption]);
            LogLevel.SetLevel(LogLevel.Warning,_options[LogDefaultOptions.WarningOption]);
            LogLevel.SetLevel(LogLevel.Info,_options[LogDefaultOptions.InfoOption]);
            LogLevel.SetLevel(LogLevel.Event,_options[LogDefaultOptions.EventOption]);
            LogLevel.SetLevel(LogLevel.Debug,_options[LogDefaultOptions.DebugOption]);
            LogLevel.SetLevel(LogLevel.All, _options[LogDefaultOptions.AllOption]);
            LogLevel.SetDefault(_options["LogLevel"]);
            
            if (_collector != null)
            {
                _collector.ShutDown();
            }
            _collector = new LogCollector(new ConcurrentQueued<LogItem>(new PoolThreadStarter()),
                new LogWriterFactory(_options).GetWriter());

            if (_container == null)
            {
                _container = new LoggerContainer(category => new CheckingLogger(category, _collector.Send, LogLevel.Default));
            }
            else
            {
                _container.SetSender(_collector.Send);
            }
            _container.DefaultCategory = _options.Get("LogDefaultCategory", "Default");
            _init = true;
        }

        /// <summary>
        /// Sets default log level for all new loggers.
        /// </summary>
        /// <param name="level"></param>
        public static void SetDefault(int level)
        {
            lock (_sync)
            {
                LogLevel.Default = level;
            }
        }

        /// <summary>
        /// Sets a log level for specified category. All the levels below it will be enabled.
        /// All the levels above - disabled. For example, setting Warning level will also enable Error 
        /// and Fatal levels. Info, Event, Debug and Detailed messages will not get into the output.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="level"></param>
        public static void SetLevel(string category, int level)
        {
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _container.SetLevel(category, level);
            }
        }

        /// <summary>
        /// Sets a single level for a single category enabled or disabled.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="level"></param>
        /// <param name="value"></param>
        public static void SetOneLevel(string category, int level, bool value)
        {
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _container.SetOneLevel(category, level, value);
            }
        }

        /// <summary>
        /// Sets a log level for all categories.
        /// </summary>
        /// <param name="level"></param>
        public static void SetLevelForAll(int level)
        {
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _container.SetLevelForAll(level);
            }
        }

        /// <summary>
        /// Sets a single level for all categories enabled or disabled.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="value"></param>
        public static void SetOneLevelForAll(int level, bool value)
        {
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _container.SetOneLevelForAll(level, value);
            }
        }

        /// <summary>
        /// Gets a logger for specified category. For example module name or a class name.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static ILogger GetLogger(string category)
        {
            ILogger logger;
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                logger = _container.GetLogger(category);
            }
            return logger;
        }

        /// <summary>
        /// Self explanatory. Gets a null logger that will not send any logs.
        /// </summary>
        /// <returns></returns>
        public static ILogger GetNullLogger()
        {
            return _nullLogger;
        }

        /// <summary>
        /// Shuts down a thread used by the logger.
        /// </summary>
        public static void ShutDown()
        {
            lock (_sync)
            {
                if (_init)
                {
                    _collector.ShutDown();
                }
            }
        }

        /// <summary>
        /// Adds an ID to ID filter. Used to only log messages for specified ID,
        /// user ID for example.
        /// </summary>
        /// <param name="id"></param>
        public static void FilterAddID(int id)
        {
            if (id == 0)
            {
                return;
            }
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _collector.FilterAddID(id);
            }
        }

        /// <summary>
        /// Removes ID from ID filter, this ID is no longer gets monitored.
        /// As soon as ID filter becomes empty, all the entries will be logged.
        /// </summary>
        /// <param name="id"></param>
        public static void FilterRemoveID(int id)
        {
            if (id == 0)
            {
                return;
            }
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _collector.FilterRemoveID(id);
            }
        }

        /// <summary>
        /// Clears ID filter. All the entries will be logged.
        /// </summary>
        public static void FilterClear()
        {
            lock (_sync)
            {
                if (!_init)
                {
                    InitBody();
                }
                _collector.FilterClear();
            }
        }
    }
}
