using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;

namespace SharpLogger
{
    class BasicLogConstructor : ILogConstructor
    {

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
            sb.Append(LogLevel.GetLevel(item.level));
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
