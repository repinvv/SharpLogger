using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpLogger;
using SharpLogger.Loggers;

namespace LoggerTest
{
   class LoggerMock : IInternalLogger
   {
      public bool levelValue = false;
      public int level;
      public string category;

      public LoggerMock(string category, int level)
      {
         this.category = category;
         this.level = level;
      }

      public void SetLevel(int level)
      {
         this.level = level;
      }

      public bool this[int level]
      {
         set { this.level = level; this.levelValue = value; }
      }

      public void Always(string message)
      {
         throw new NotImplementedException();
      }

      public void Fatal(string message, Exception ex = null)
      {
         throw new NotImplementedException();
      }

      public void Error(string message, Exception ex = null)
      {
         throw new NotImplementedException();
      }

      public void Warning(string message, int id = 0, Exception ex = null)
      {
         throw new NotImplementedException();
      }

      public void Warning(string message, int[] id, Exception ex = null)
      {
         throw new NotImplementedException();
      }

      public void Info(string message, int id = 0)
      {
         throw new NotImplementedException();
      }

      public void Info(string message, int[] id)
      {
         throw new NotImplementedException();
      }

      public void Event(string message, int id = 0)
      {
         throw new NotImplementedException();
      }

      public void Event(string message, int[] id)
      {
         throw new NotImplementedException();
      }

      public void Debug(string message, int id = 0)
      {
         throw new NotImplementedException();
      }

      public void Debug(string message, int[] id)
      {
         throw new NotImplementedException();
      }

      public void Detailed(string message, int id = 0)
      {
         throw new NotImplementedException();
      }

      public void Detailed(string message, int[] id)
      {
         throw new NotImplementedException();
      }

      public void SetSender(Sender sender)
      {
          throw new NotImplementedException();
      }


      public void Warning(string message, Exception ex)
      {
          throw new NotImplementedException();
      }
   }
}
