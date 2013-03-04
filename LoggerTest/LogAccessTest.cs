using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LoggerTest
{


   /// <summary>
   ///This is a test class for LogManagerTest and is intended
   ///to contain all LogManagerTest Unit Tests
   ///</summary>
   [TestClass()]
   public class LogAccessTest
   {

      private TestContext testContextInstance;

      /// <summary>
      ///Gets or sets the test context which provides
      ///information about and functionality for the current test run.
      ///</summary>
      public TestContext TestContext
      {
         get
         {
            return testContextInstance;
         }
         set
         {
            testContextInstance = value;
         }
      }

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

      /// <summary>
      ///A test for GetLogger
      ///</summary>
      [TestMethod()]
      public void GetLoggerTest()
      {
         string Category1 = "Some cat";
         string Category2 = "Another cat";
         ILogger first, second, third, nullLog;
         first = LogAccess.GetLogger(Category1);
         second = LogAccess.GetLogger(Category2);
         third = LogAccess.GetLogger(Category1);
         nullLog = LogAccess.GetNullLogger();
         Assert.IsNotNull(first);
         Assert.IsNotNull(second);
         Assert.AreNotEqual(first, nullLog);
         Assert.AreNotEqual(second, nullLog);
         Assert.AreNotEqual(first, second);
         Assert.AreEqual(first, third);
         TryLogger(first);
      }

      /// <summary>
      ///A test for GetNullLogger
      ///</summary>
      [TestMethod()]
      public void GetNullLoggerTest()
      {
         ILogger first, second;
         first = LogAccess.GetNullLogger();
         second = LogAccess.GetNullLogger();
         Assert.IsNotNull(first);
         Assert.IsNotNull(second);
         Assert.AreEqual(first, second);
         TryLogger(first);
      }

      void TryLogger(ILogger logger)
      {
         bool thrown = false;
         string message = "msg";
         try
         {
            logger.Always(message);
            logger.Debug(message);
            logger.Detailed(message);
            logger.Error(message);
            logger.Event(message);
            logger.Fatal(message);
            logger.Info(message);
            logger.Warning(message);
         }
         catch (Exception)
         {
            thrown = true;
         }
         Assert.IsFalse(thrown);
      }

   }
}
