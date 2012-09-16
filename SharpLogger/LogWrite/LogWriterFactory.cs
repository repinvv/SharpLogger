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
         switch (options["LogWriteTarget"].ToLower())
         {
            case "file":
            default:
               return new LogFileWriter((new LogConstructorFactory(options)).GetLogConstructor(), options);               
         }
      }
   }
}
