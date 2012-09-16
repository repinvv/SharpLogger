using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManualTest
{
   struct LogItem
   {
      public readonly String category;
      public readonly String message;
      public readonly int[] ids;
      public readonly uint level;
      public readonly Exception ex;
      public readonly DateTime time;

      public LogItem(String category, uint level, String line, int id = 0, Exception ex = null)
      {
         time = DateTime.Now;
         this.category = category;
         this.level = level;
         this.message = line;
         this.ids = new int[]{id};
         this.ex = ex;
      }

      public LogItem(String category, uint level, String line, int[] ids, Exception ex = null)
      {
         time = DateTime.Now;
         this.category = category;
         this.level = level;
         this.message = line;
         this.ids = ids;
         this.ex = ex;
      }

   }
}
