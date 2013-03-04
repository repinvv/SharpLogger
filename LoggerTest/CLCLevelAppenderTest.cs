using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using SharpLogger.LogConstruction;
using SharpOptions;

namespace LoggerTest
{

   
    /// <summary>
    ///This is a test class for CLCLevelAppenderTest and is intended
    ///to contain all CLCLevelAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCLevelAppenderTest : LogConstructorTest
   {

      internal override ILogConstructor CreateLogConstructor()
      {
         return new LevelAppender();
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed.Length > 3;
      }
   }
}
