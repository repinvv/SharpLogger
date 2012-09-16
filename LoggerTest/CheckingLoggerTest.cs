using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CheckingLoggerTest and is intended
    ///to contain all CheckingLoggerTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CheckingLoggerTest : LoggerTest
   {

      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>

      #region Additional test attributes
      // 
      //You can use the following additional attributes as you write your tests:
      //
      //Use ClassInitialize to run code before running the first test in the class
      //[ClassInitialize()]
      //public static void MyClassInitialize(TestContext testContext)
      //{
      //}
      //
      //Use ClassCleanup to run code after all tests in a class have run
      //[ClassCleanup()]
      //public static void MyClassCleanup()
      //{
      //}
      //
      //Use TestInitialize to run code before running each test
      //[TestInitialize()]
      //public void MyTestInitialize()
      //{
      //}
      //
      //Use TestCleanup to run code after each test has run
      //[TestCleanup()]
      //public void MyTestCleanup()
      //{
      //}
      //
      #endregion




      internal override InternalLogger CreateLogger()
      {
         return new CheckingLogger(category, Send, LogLevel.Default);
      }
   }
}
