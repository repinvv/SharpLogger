using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;

namespace SharpLogger
{
   class CLCLevelAppender : ConfigurableLogConstructor
   {
      string[] levels = new string[LogLevel.Total];

      public CLCLevelAppender(IOptions options)
      {
         levels[LogLevel.Always] = options[LogDefaultOptions.AlwaysOption];
         levels[LogLevel.Fatal] = options[LogDefaultOptions.FatalOption];
         levels[LogLevel.Error] = options[LogDefaultOptions.ErrorOption];
         levels[LogLevel.Warning] = options[LogDefaultOptions.WarningOption];
         levels[LogLevel.Info] = options[LogDefaultOptions.InfoOption];
         levels[LogLevel.Event] = options[LogDefaultOptions.EventOption];
         levels[LogLevel.Debug] = options[LogDefaultOptions.DebugOption];
         levels[LogLevel.All] = options[LogDefaultOptions.AllOption];
      }

      public override void ConstructLine(StringBuilder sb, LogItem item)
      {
         sb.Append(levels[item.level]);
         next.ConstructLine(sb, item);
      }
   }
}
