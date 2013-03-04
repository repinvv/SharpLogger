using SharpLogger.LogConstruction;
using SharpOptions;

namespace SharpLogger.LogWrite
{

    class LogWriterFactory
    {
        IOptions _options;

        public LogWriterFactory(IOptions options)
        {
            _options = options;
        }

        public ILogWriter GetWriter()
        {
            switch (_options["LogWriteTarget"])
            {
                case "Database":
                    return new LogDatabaseWriter(_options);
                default:
                    return new LogFileWriter((new LogConstructorFactory(_options)).GetLogConstructor(), _options);
            }
        }
    }
}
