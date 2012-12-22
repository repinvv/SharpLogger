using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;

namespace SharpLogger
{
   class LogConstructorFactory
   {
      IOptions options;
      ConfigurableLogConstructor first, current;
      StringBuilder buffer = new StringBuilder(), sb;

      public LogConstructorFactory(IOptions options)
      {
         this.options = options;  
      }


      void Escaped(char val)
      {         
         switch (val)
         {
            case 'd':
               AddConstructor(
                  new CLCDateTimeAppender(
                     options["LogDateTimeFormat"],
                     options["LogMilisecondsFormat"]));
               break;
            case 'l':
               AddConstructor(new CLCLevelAppender(options));
               break;
            case 'i':
               AddConstructor(new CLCIdAppender(options["LogIDSplitChar"]));
               break;
            case 'c':
               AddConstructor(new CLCCategoryAppender());
               break;
            case 'm':
               AddConstructor(new CLCMessageAppender());
               break;
            case 'e':
               AddConstructor(new CLCExceptionAppender());
               break;
            case 't':
               AddConstructor(new CLCStackTraceAppender());
               break;
            case 'r':
               buffer.Append('\r');
               break;
            case 'n':
               buffer.Append('\n');
               break;
            default:
               buffer.Append(val);
               break;
         }
      }

      void AddConstructor(ConfigurableLogConstructor constructor)
      {
         if (buffer.Length > 0)
         {
            var output = buffer.ToString();
            buffer.Clear();
            AddConstructor(new CLCStringAppender(output));
         }
         if (first == null)
         {
            first = constructor;
            current = constructor;
         }
         else
         {
            current.Chain(constructor);
            current = constructor;
         }
      }

      public ILogConstructor GetLogConstructor()
      {
         string messageFormat = options["LogMessageFormat"];
         if (messageFormat == "basic" || messageFormat.Length < 5)
            return new BasicLogConstructor(options);
         first = null;
         sb = new StringBuilder();
         buffer.Clear();
         for (int n = 0; n < messageFormat.Length; n++)
         {
            if (n < messageFormat.Length - 1 && messageFormat[n] == '\\')
            {
               Escaped(messageFormat[n + 1]);
               n++;
            }
            else
            {
               buffer.Append(messageFormat[n]);
            }
         }
         AddConstructor(new CLCResultReturner());
         sb = null;
         return first;
      }
   }
}
