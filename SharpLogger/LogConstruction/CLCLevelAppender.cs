using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
namespace SharpLogger
{
   class CLCLevelAppender : ConfigurableLogConstructor
   {
      string[] levels = new string[LogLevel.Total];

      public CLCLevelAppender()
      {
         levels[LogLevel.Always] = LogConfig.Options[LogDefaultOptions.AlwaysOption];
         levels[LogLevel.Fatal] = LogConfig.Options[LogDefaultOptions.FatalOption];
         levels[LogLevel.Error] = LogConfig.Options[LogDefaultOptions.ErrorOption];
         levels[LogLevel.Warning] = LogConfig.Options[LogDefaultOptions.WarningOption];
         levels[LogLevel.Info] = LogConfig.Options[LogDefaultOptions.InfoOption];
         levels[LogLevel.Event] = LogConfig.Options[LogDefaultOptions.EventOption];
         levels[LogLevel.Debug] = LogConfig.Options[LogDefaultOptions.DebugOption];
         levels[LogLevel.All] = LogConfig.Options[LogDefaultOptions.AllOption];
      }

      public override void ConstructLine(StringBuilder sb, LogItem item)
      {
         sb.Append(levels[item.level]);
         next.ConstructLine(sb, item);
      }
   }
}
