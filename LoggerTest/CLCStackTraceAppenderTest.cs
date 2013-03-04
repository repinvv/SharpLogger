using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using SharpLogger.LogConstruction;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCStackTraceAppenderTest and is intended
    ///to contain all CLCStackTraceAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCStackTraceAppenderTest : LogConstructorTest
   {
      internal override ILogConstructor CreateLogConstructor()
      {
         return new StackTraceExceptionAppender();
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed.Contains("LoggerTest");
      }
   }
}
