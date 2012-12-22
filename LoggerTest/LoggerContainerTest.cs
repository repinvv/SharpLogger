using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Autofac;
using Moq;
using System.Threading;

namespace LoggerTest
{

   /// <summary>
   ///This is a test class for LoggerFactoryTest and is intended
   ///to contain all LoggerFactoryTest Unit Tests
   ///</summary>
   [TestClass()]
   public class LoggerContainerTest
   {
      IContainer testContainer;



      public LoggerContainerTest()
      {
         var builder = new ContainerBuilder();

         builder
            .Register<LoggerContainer>((c, p) => new LoggerContainer(new OptionsMock(),
                (string name) => new LoggerMock(name, LogLevel.Info)));

         builder
            .RegisterType<LoggerMock>()
            .As<InternalLogger>();

         testContainer = builder.Build();
      }


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
         string cat1 = "cat1";
         string cat2 = "cat2";
         LoggerContainer lc =
         testContainer
            .Resolve<LoggerContainer>();
         var logger1 = lc.GetLogger(cat1);
         var logger2 = lc.GetLogger(cat2);
         var logger3 = lc.GetLogger(cat1);
         Assert.IsNotNull(logger1);
         Assert.IsNotNull(logger2);
         Assert.IsNotNull(logger3);
         Assert.AreEqual(logger1, logger3);
         Assert.AreEqual(cat1, ((LoggerMock)logger1).category);
         Assert.AreNotEqual(logger1, logger2);
         var logger = logger1 as LoggerMock;
         Assert.IsNotNull(logger);
         Assert.AreEqual(cat1, logger.category);
      }

      /// <summary>
      ///A test for SetLevel
      ///</summary>
      [TestMethod()]
      public void SetLevelTest()
      {
         string cat1 = "cat1";
         string cat2 = "cat2";
         LoggerContainer lc =
            testContainer
            .Resolve<LoggerContainer>();
         var logger1 = lc.GetLogger(cat1) as LoggerMock;
         var logger2 = lc.GetLogger(cat2) as LoggerMock;
         lc.SetLevel(cat1, LogLevel.Debug);
         Assert.AreEqual(LogLevel.Debug, logger1.level);
         Assert.AreNotEqual(LogLevel.Debug, logger2.level);
      }

      /// <summary>
      ///A test for SetLevelForAll
      ///</summary>
      [TestMethod()]
      public void SetLevelForAllTest()
      {
         string cat1 = "cat1";
         string cat2 = "cat2";
         LoggerContainer lc =
            testContainer
            .Resolve<LoggerContainer>();
         var logger1 = lc.GetLogger(cat1) as LoggerMock;
         var logger2 = lc.GetLogger(cat2) as LoggerMock;
         lc.SetLevelForAll(LogLevel.Debug);
         Assert.AreEqual(LogLevel.Debug, logger1.level);
         Assert.AreEqual(LogLevel.Debug, logger2.level);
      }

      /// <summary>
      ///A test for SetOneLevel
      ///</summary>
      [TestMethod()]
      public void SetOneLevelTest()
      {
         string cat1 = "cat1";
         string cat2 = "cat2";
         LoggerContainer lc =
            testContainer
            .Resolve<LoggerContainer>();
         var logger1 = lc.GetLogger(cat1) as LoggerMock;
         var logger2 = lc.GetLogger(cat2) as LoggerMock;
         lc.SetOneLevel(cat1, LogLevel.Debug, true);
         Assert.AreEqual(LogLevel.Debug, logger1.level);
         Assert.IsTrue(logger1.levelValue);
         Assert.AreNotEqual(LogLevel.Debug, logger2.level);
         Assert.IsFalse(logger2.levelValue);
      }

      /// <summary>
      ///A test for SetOneLevelForAll
      ///</summary>
      [TestMethod()]
      public void SetOneLevelForAllTest()
      {
         string cat1 = "cat1";
         string cat2 = "cat2";
         LoggerContainer lc =
            testContainer
            .Resolve<LoggerContainer>();
         var logger1 = lc.GetLogger(cat1) as LoggerMock;
         var logger2 = lc.GetLogger(cat2) as LoggerMock;
         lc.SetOneLevelForAll(LogLevel.Debug, true);
         Assert.AreEqual(LogLevel.Debug, logger1.level);
         Assert.IsTrue(logger1.levelValue);
         Assert.AreEqual(LogLevel.Debug, logger2.level);
         Assert.IsTrue(logger2.levelValue);
      }
   }
}
