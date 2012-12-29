using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class LogItem
    {
        public readonly DateTime time;
        public readonly String category;
        public readonly int level;
        public readonly int[] ids;
        public readonly String message;
        public readonly Exception ex;        

        public LogItem(String category, int level, String message, int id = 0, Exception ex = null)
        {
            time = DateTime.Now;
            this.category = category;
            this.level = LogLevel.isLevelValid(level) ? level : LogLevel.Debug;
            this.message = message;
            this.ids = new int[] { id };
            this.ex = ex;
        }

        public LogItem(String category, int level, String message, int[] ids, Exception ex = null)
        {
            time = DateTime.Now;
            this.category = category;
            this.level = LogLevel.isLevelValid(level) ? level : LogLevel.Debug;
            this.message = message;
            this.ids = ids;
            this.ex = ex;
        }

    }
}
