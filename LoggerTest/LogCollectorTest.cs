using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using SharpLogger.LogWrite;

namespace LoggerTest
{
    /// <summary>
    ///This is a test class for LogCollectorTest and is intended
    ///to contain all LogCollectorTest Unit Tests
    ///</summary>
   [TestClass()]
   public class LogCollectorTest
   {
      string category = "cat";
      int level = LogLevel.Info;
      string msg = "message";
      int[] ids = new int[] { 3, 4, 5 };
      string exception = "exception";
      int timeout = 101;

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

      Exception GetException()
      {
         Exception ex = null;
         try
         {
            throw new Exception(exception);
         }
         catch (Exception ex1)
         {
            ex = ex1;
         }
         return ex;
      }

      /// <summary>
      ///A test for Send
      ///</summary>
      [TestMethod()]
      public void SendTest()
      {
         LogItem message = new LogItem(category, level, msg, ids, GetException());
         var qThread = new Mock<IQueued<LogItem>>(MockBehavior.Strict);
         var writer = new Mock<ILogWriter>(MockBehavior.Strict);

         qThread
            .Setup(s => s.Send(It.IsAny<LogItem>()));
         writer
            .Setup(s => s.GetTimeout())
            .Returns(timeout);
         qThread
            .Setup(s => s.SetTimeout(It.IsAny<int>()));

         LogCollector target = new LogCollector(qThread.Object, writer.Object);

         qThread
            .Verify(s => s.SetTimeout(It.Is<int>(a => a == timeout)));

         target.Send(message);

         qThread
            .Verify(s => s.Send(It.Is<LogItem>(i => i.Equals(message))), Times.Once());

      }

      [TestMethod()]
      public void SendTest2()
      {
         LogItem message = new LogItem(category, level, msg, ids, GetException());
         var qThread = new Mock<IQueued<LogItem>>(MockBehavior.Strict);
         var writer = new Mock<ILogWriter>(MockBehavior.Strict);

         writer
            .Setup(s => s.Write(It.IsAny<LogItem>()));
         writer
            .Setup(s => s.GetTimeout())
            .Returns(timeout);
         qThread
            .Setup(s => s.SetTimeout(It.IsAny<int>()));

         LogCollector target = new LogCollector(qThread.Object, writer.Object);
         qThread.Raise(s => s.OnReceive += null, message);

         writer
            .Verify(s => s.Write(It.Is<LogItem>(a => a.Equals(message))), Times.Once());

      }

      /// <summary>
      ///A test for FilterAddID
      ///</summary>
      [TestMethod()]
      public void FilterAddIDTest()
      {
         LogItem message = new LogItem(category, level, msg, ids, GetException());
         var qThread = new Mock<IQueued<LogItem>>(MockBehavior.Strict);
         var writer = new Mock<ILogWriter>(MockBehavior.Strict);

         writer
            .Setup(s => s.Write(It.IsAny<LogItem>()));
         writer
            .Setup(s => s.GetTimeout())
            .Returns(timeout);
         qThread
            .Setup(s => s.SetTimeout(It.IsAny<int>()));

         LogCollector target = new LogCollector(qThread.Object, writer.Object);
         target.FilterAddID(1);
         qThread.Raise(s => s.OnReceive += null, message);

         writer
           .Verify(s => s.Write(It.IsAny<LogItem>()), Times.Never());

         target.FilterAddID(3);
         target.FilterAddID(1);
         qThread.Raise(s => s.OnReceive += null, message);

         writer
            .Verify(s => s.Write(It.Is<LogItem>(a => a.Equals(message))), Times.Once());
      }

      /// <summary>
      ///A test for FilterClear
      ///</summary>
      [TestMethod()]
      public void FilterClearTest()
      {
         LogItem message = new LogItem(category, level, msg, ids, GetException());
         var qThread = new Mock<IQueued<LogItem>>(MockBehavior.Strict);
         var writer = new Mock<ILogWriter>(MockBehavior.Strict);

         writer
            .Setup(s => s.Write(It.IsAny<LogItem>()));
         writer
            .Setup(s => s.GetTimeout())
            .Returns(timeout);
         qThread
            .Setup(s => s.SetTimeout(It.IsAny<int>()));

         LogCollector target = new LogCollector(qThread.Object, writer.Object);
         target.FilterAddID(1);
         qThread.Raise(s => s.OnReceive += null, message);

         writer
           .Verify(s => s.Write(It.IsAny<LogItem>()), Times.Never());

         target.FilterClear();
         qThread.Raise(s => s.OnReceive += null, message);

         writer
            .Verify(s => s.Write(It.Is<LogItem>(a => a.Equals(message))), Times.Once());
      }

      /// <summary>
      ///A test for FilterRemoveID
      ///</summary>
      [TestMethod()]
      public void FilterRemoveIDTest()
      {
         LogItem message = new LogItem(category, level, msg, ids, GetException());
         var qThread = new Mock<IQueued<LogItem>>(MockBehavior.Strict);
         var writer = new Mock<ILogWriter>(MockBehavior.Strict);

         writer
            .Setup(s => s.Write(It.IsAny<LogItem>()));
         writer
            .Setup(s => s.GetTimeout())
            .Returns(timeout);
         qThread
            .Setup(s => s.SetTimeout(It.IsAny<int>()));

         LogCollector target = new LogCollector(qThread.Object, writer.Object);
         target.FilterAddID(1);
         target.FilterAddID(3);
         qThread.Raise(s => s.OnReceive += null, message);

         writer
           .Verify(s => s.Write(It.Is<LogItem>(a => a.Equals(message))), Times.Once());

         target.FilterRemoveID(3);
         qThread.Raise(s => s.OnReceive += null, message);

         writer
            .Verify(s => s.Write(It.Is<LogItem>(a => a.Equals(message))), Times.Once());     
      }


      /// <summary>
      ///A test for ShutDown
      ///</summary>
      [TestMethod()]
      public void ShutDownTest()
      {
         LogItem message = new LogItem(category, level, msg, ids, GetException());
         var qThread = new Mock<IQueued<LogItem>>(MockBehavior.Strict);
         var writer = new Mock<ILogWriter>(MockBehavior.Strict);

         qThread
            .Setup(s => s.Terminate());
         writer
            .Setup(s => s.GetTimeout())
            .Returns(timeout);
         qThread
            .Setup(s => s.SetTimeout(It.IsAny<int>()));

         LogCollector target = new LogCollector(qThread.Object, writer.Object);
         target.ShutDown();

         qThread
            .Verify(s => s.Terminate(), Times.Once());
      }
   }
}
