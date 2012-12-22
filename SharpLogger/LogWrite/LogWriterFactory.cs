using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;

namespace SharpLogger
{
    enum WriterType
    {
        Default = 0,
        File,
        Database
    }

    class LogWriterFactory
    {
        IOptions options;

        public LogWriterFactory(IOptions options)
        {
            this.options = options;
        }

        ILogWriter GetDefaultWriter()
        {
            switch (options["LogWriteTarget"].ToLower())
            {
                case "file":
                default:
                    return new LogFileWriter((new LogConstructorFactory(options)).GetLogConstructor(), options);
            }
        }

        public ILogWriter GetWriter(WriterType writerType = WriterType.Default)
        {
            switch (writerType)
            {
                case WriterType.Default:
                    return GetDefaultWriter();
                case WriterType.Database:
                    return null;
                case WriterType.File:
                default:
                    return new LogFileWriter((new LogConstructorFactory(options)).GetLogConstructor(), options);
            }
        }
    }
}
