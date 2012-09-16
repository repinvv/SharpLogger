using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using Autofac;
using System.Collections.Generic;

namespace SharpLogger
{
   //entry point static class for the logger
   public static class LogAccess
   {
      static Logger nullLogger = new NullLogger();

      static LogAccess()
      {
      }

      public static void SetDefault(int Level)
      {
         LogLevel.Default = Level;
      }

      public static void SetLevel(string category, int level)
      {
         LogConfig
            .Container
            .Resolve<LoggerContainer>()
            .SetLevel(category, level);
      }

      public static void SetOneLevel(string category, int level, bool value)
      {
         LogConfig
            .Container
            .Resolve<LoggerContainer>()
            .SetOneLevel(category, level, value);
      }

      public static void SetLevelForAll(int level)
      {
         LogConfig
            .Container
            .Resolve<LoggerContainer>()
            .SetLevelForAll(level);
      }

      public static void SetOneLevelForAll(int level, bool value)
      {
         LogConfig
            .Container
            .Resolve<LoggerContainer>()
            .SetOneLevelForAll(level, value);
      }

      public static Logger GetLogger(string category)
      {
         return LogConfig
            .Container
            .Resolve<LoggerContainer>()
            .GetLogger(category);
      }

      public static Logger GetNullLogger()
      {
         return nullLogger;
      }

      public static void ShutDown()
      {
         LogConfig
            .Container
            .Resolve<LogCollector>()
            .ShutDown();
      }

      public static void FilterAddID(int id)
      {
         if (id == 0)
         {
            return;
         }
         LogConfig
            .Container
            .Resolve<LogCollector>()
            .FilterAddID(id);
      }

      public static void FilterRemoveID(int id)
      {
         if (id == 0)
         {
            return;
         }
         LogConfig
            .Container
            .Resolve<LogCollector>()
            .FilterRemoveID(id);
      }

      public static void FilterClear()
      {
         LogConfig
            .Container
            .Resolve<LogCollector>()
            .FilterClear();
      }

   }
}
