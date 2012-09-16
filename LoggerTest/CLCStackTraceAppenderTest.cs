using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

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
         var item = new CLCStackTraceAppender();
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed.Contains("LoggerTest");
      }
   }
}
