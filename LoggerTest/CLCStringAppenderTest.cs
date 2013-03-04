using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using SharpLogger.LogConstruction;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCStringAppenderTest and is intended
    ///to contain all CLCStringAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCStringAppenderTest : LogConstructorTest
   {
      string staticString = "staticString";


      internal override ILogConstructor CreateLogConstructor()
      {
         return new StringAppender(staticString);
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed == staticString;
      }
   }
}
