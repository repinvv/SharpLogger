using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;

namespace SharpLogger
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
                case "File":
                default:
                    return new LogFileWriter((new LogConstructorFactory(options)).GetLogConstructor(), options);
            }
        }
    }
}
