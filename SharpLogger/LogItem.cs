using System;

namespace SharpLogger
{
    class LogItem
    {
        public readonly DateTime Time;
        public readonly String Category;
        public readonly int Level;
        public readonly int[] Ids;
        public readonly String Message;
        public readonly Exception Ex;        

        public LogItem(String category, int level, String message, int id = 0, Exception ex = null)
        {
            Time = DateTime.Now;
            Category = category;
            Level = LogLevel.isLevelValid(level) ? level : LogLevel.Debug;
            Message = message;
            Ids = new int[] { id };
            Ex = ex;
        }

        public LogItem(String category, int level, String message, int[] ids, Exception ex = null)
        {
            Time = DateTime.Now;
            Category = category;
            Level = LogLevel.isLevelValid(level) ? level : LogLevel.Debug;
            Message = message;
            Ids = ids;
            Ex = ex;
        }

    }
}
