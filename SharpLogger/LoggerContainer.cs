using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Autofac;
using SharpOptions;
namespace SharpLogger
{
   class LoggerContainer
   {
      IComponentContext context;
      ConcurrentDictionary<string, InternalLogger> loggers;

      public LoggerContainer(IComponentContext context)
      {
         this.context = context;
         loggers = new ConcurrentDictionary<string, InternalLogger>();
      }

      private InternalLogger CreateLogger(string category)
      {
         return context
            .Resolve<InternalLogger>(new NamedParameter[]{
               new NamedParameter("category", category),
               new NamedParameter("level",LogLevel.Default)
            });
      }

      public InternalLogger GetLogger(string category)
      {
         if (category == null || category == string.Empty)
         {
            category = context
               .Resolve<IOptions>()
               .Get("LogDefaultCategory", "Default");
         }
         InternalLogger logger = null;
         lock (loggers)
         {
            if (loggers.TryGetValue(category, out logger))
            {
               return logger;
            }
            var newlogger = CreateLogger(category);
            while (!loggers.TryGetValue(category, out logger))
            {
               loggers.TryAdd(category, newlogger);
            }
         }
         return logger;
      }

      public void SetLevel(string category, int level)
      {
         InternalLogger logger;
         if (loggers.TryGetValue(category, out logger))
         {
            logger.SetLevel(level);
         }
      }

      public void SetOneLevel(string category, int level, bool value)
      {
         InternalLogger logger;
         if (loggers.TryGetValue(category, out logger))
         {
            logger[level] = value;
         }
      }

      public void SetLevelForAll(int level)
      {
         foreach (var x in loggers)
            x.Value.SetLevel(level);
      }

      public void SetOneLevelForAll(int level, bool value)
      {
         foreach (var x in loggers)
            x.Value[level] = value;
      }

   }
}
