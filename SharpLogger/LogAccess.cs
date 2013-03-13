using SharpLogger.LogWrite;
using SharpLogger.Loggers;
using SharpOptions;

namespace SharpLogger
{
    //entry point static class for the logger
    public static class LogAccess
    {
        static bool init;
        static object sync = new object();
        static Logger nullLogger = new NullLogger();
        static LogCollector collector;
        static LoggerContainer container;
        static IOptions options;

        static LogAccess()
        {
        }

        /// <summary>
        /// Initiates logging.
        /// </summary>
        static public void Init()
        {
            lock (sync)
            {
                options = null;
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
            lock (sync)
            {
                LogAccess.options = options;
                InitBody();
            }
        }

        static void InitBody()
        {
            if (options == null)
            {
                options = new Options("", "Log");
            }
            LogDefaultOptions.AddDefaultOptions(options);
            LogLevel.SetLevel(LogLevel.Always, options[LogDefaultOptions.AlwaysOption]);
            LogLevel.SetLevel(LogLevel.Fatal,options[LogDefaultOptions.FatalOption]);
            LogLevel.SetLevel(LogLevel.Error,options[LogDefaultOptions.ErrorOption]);
            LogLevel.SetLevel(LogLevel.Warning,options[LogDefaultOptions.WarningOption]);
            LogLevel.SetLevel(LogLevel.Info,options[LogDefaultOptions.InfoOption]);
            LogLevel.SetLevel(LogLevel.Event,options[LogDefaultOptions.EventOption]);
            LogLevel.SetLevel(LogLevel.Debug,options[LogDefaultOptions.DebugOption]);
            LogLevel.SetLevel(LogLevel.All, options[LogDefaultOptions.AllOption]);
            LogLevel.SetDefault(options["LogLevel"]);
            
            if (collector != null)
            {
                collector.ShutDown();
            }
            collector = new LogCollector(new ConcurrentQueued<LogItem>(new PoolThreadStarter()),
                new LogWriterFactory(options).GetWriter());

            if (container == null)
            {
                container = new LoggerContainer(category => new CheckingLogger(category, collector.Send, LogLevel.Default));
            }
            else
            {
                container.SetSender(collector.Send);
            }
            container.DefaultCategory = options.Get("LogDefaultCategory", "Default");
            init = true;
        }

        /// <summary>
        /// Sets default log level for all new loggers.
        /// </summary>
        /// <param name="level"></param>
        public static void SetDefault(int level)
        {
            lock (sync)
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
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                container.SetLevel(category, level);
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
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                container.SetOneLevel(category, level, value);
            }
        }

        /// <summary>
        /// Sets a log level for all categories.
        /// </summary>
        /// <param name="level"></param>
        public static void SetLevelForAll(int level)
        {
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                container.SetLevelForAll(level);
            }
        }

        /// <summary>
        /// Sets a single level for all categories enabled or disabled.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="value"></param>
        public static void SetOneLevelForAll(int level, bool value)
        {
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                container.SetOneLevelForAll(level, value);
            }
        }

        /// <summary>
        /// Gets a logger for specified category. For example module name or a class name.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static Logger GetLogger(string category)
        {
            Logger logger;
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                logger = container.GetLogger(category);
            }
            return logger;
        }

        /// <summary>
        /// Self explanatory. Gets a null logger that will not send any logs.
        /// </summary>
        /// <returns></returns>
        public static Logger GetNullLogger()
        {
            return nullLogger;
        }

        /// <summary>
        /// Shuts down a thread used by the logger.
        /// </summary>
        public static void ShutDown()
        {
            lock (sync)
            {
                if (init)
                {
                    collector.ShutDown();
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
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                collector.FilterAddID(id);
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
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                collector.FilterRemoveID(id);
            }
        }

        /// <summary>
        /// Clears ID filter. All the entries will be logged.
        /// </summary>
        public static void FilterClear()
        {
            lock (sync)
            {
                if (!init)
                {
                    InitBody();
                }
                collector.FilterClear();
            }
        }
    }
}
