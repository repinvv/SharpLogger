using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
   class BasicLogConstructor : ILogConstructor
   {
      String[] levels;

      public BasicLogConstructor()
      {
         levels = new string[LogLevel.Total];
         levels[LogLevel.Always] = LogConfig.Options[LogDefaultOptions.AlwaysOption];
         levels[LogLevel.Fatal] = LogConfig.Options[LogDefaultOptions.FatalOption];
         levels[LogLevel.Error] = LogConfig.Options[LogDefaultOptions.ErrorOption];
         levels[LogLevel.Warning] = LogConfig.Options[LogDefaultOptions.WarningOption];
         levels[LogLevel.Info] = LogConfig.Options[LogDefaultOptions.InfoOption];
         levels[LogLevel.Event] = LogConfig.Options[LogDefaultOptions.EventOption];
         levels[LogLevel.Debug] = LogConfig.Options[LogDefaultOptions.DebugOption];
         levels[LogLevel.All] = LogConfig.Options[LogDefaultOptions.AllOption]; 
      }

      private void AppendIdString(StringBuilder sb, int[] ids)
      {
         foreach (int id in ids)
         {
            if (id != default(int))
               sb.Append(String.Format(":{0}", id));
         }
         if (ids.Length > 0)
            sb.Append(':');
      }

      public void ConstructLine(StringBuilder sb, LogItem item)
      {
         sb.Clear();
         sb.Append(item.time.ToString("dd-MM-yyyy HH.mm.ss"));
         sb.Append(String.Format(":{0:000} ", item.time.Millisecond));
         AppendIdString(sb, item.ids);
         sb.Append(' ');
         sb.Append(item.category);
         sb.Append(' ');
         sb.Append(levels[item.level]);
         sb.Append(' ');
         sb.Append(item.message);
         if (item.ex != null)
         {
            sb.Append('\n');
            sb.Append(item.ex.Message);
            sb.Append(item.ex.StackTrace);
         }
         sb.Append('\n');         
      }
   }
}
