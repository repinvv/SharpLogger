using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using SharpLogger.LogConstruction;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCMessageAppenderTest and is intended
    ///to contain all CLCMessageAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCMessageAppenderTest : LogConstructorTest
   {
      internal override ILogConstructor CreateLogConstructor()
      {
         return new MessageAppender();
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed == message;
      }
   }
}
