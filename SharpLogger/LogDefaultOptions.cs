using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using SharpOptions;

namespace SharpLogger
{
   class LogDefaultOptions
   {
      IOptions options;
      public LogDefaultOptions(IOptions options)
      {
         this.options = options;
         options.TryAdd("LogMessageFormat", "basic");
         options.TryAdd("LogDateTimeFormat", "dd-MM-yyyy HH.mm.ss");
         options.TryAdd("LogMilisecondsFormat", ":{0:000}");
         options.TryAdd("LogIDSplitChar", ":");

         options.TryAdd(AlwaysOption, "Безусловное сообщение");
         options.TryAdd(FatalOption, "Критическая ошибка");
         options.TryAdd(ErrorOption, "Ошибка");
         options.TryAdd(WarningOption, "Внимание");
         options.TryAdd(InfoOption, "Инфо");
         options.TryAdd(EventOption, "Событие");
         options.TryAdd(DebugOption, "Отладка");
         options.TryAdd(AllOption, "Детальная отладка");
      }

      public IOptions GetOptions()
      {
         return options;
      }

      public const string AlwaysOption = "LogAlwaysString";
      public const string FatalOption = "LogFatalString";
      public const string ErrorOption = "LogErrorString";
      public const string WarningOption = "LogWarningString";
      public const string InfoOption = "LogInfoString";
      public const string EventOption = "LogEventString";
      public const string DebugOption = "LogDebugString";
      public const string AllOption = "LogAllString";
      //"LogDefaultCategory"
      //"LogDateTimeFormat"
      //"LogMilisecondsFormat"
      //"LogIDSplitChar"
      //"LogFileFlush"
      //"LogFileAppend"
      //"LogRotationSizeMb"
      //"LogRotationFiles"
   }
}
