using SharpLogger.LogConstruction;
using SharpOptions;

namespace SharpLogger.LogWrite
{
    class LogWriterFactory
    {
        IOptions options;

        public LogWriterFactory(IOptions options)
        {
            this.options = options;
        }

        public ILogWriter GetWriter()
        {
            switch (options["LogWriteTarget"])
            {
                case "Database":
                    return new LogDatabaseWriter(options);
                default:
                    return new LogFileWriter((new LogConstructorFactory(options)).GetLogConstructor(), options);
            }
        }
    }
}
