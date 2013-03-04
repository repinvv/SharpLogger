using System;
using System.Text;

namespace SharpLogger.LogConstruction
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
            sb.Append(item.Time.ToString("dd-MM-yyyy HH.mm.ss"));
            sb.Append(String.Format(":{0:000} ", item.Time.Millisecond));
            AppendIdString(sb, item.Ids);
            sb.Append(' ');
            sb.Append(item.Category);
            sb.Append(' ');
            sb.Append(LogLevel.GetLevel(item.Level));
            sb.Append(' ');
            sb.Append(item.Message);
            if (item.Ex != null)
            {
                sb.Append('\n');
                sb.Append(item.Ex.Message);
                sb.Append(item.Ex.StackTrace);
            }
            sb.Append('\n');
        }
    }
}
