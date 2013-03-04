using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using SharpLogger.Loggers;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for LoggerTest and is intended
    ///to contain all LoggerTest Unit Tests
    ///</summary>
   [TestClass()]
   public abstract class LoggerTest
   {
      private TestContext testContextInstance;
      int calls;
      LogItem lastItem;

      protected string category = "cat";
      string message = "msg";
      int id = 1234;
      int[] ids = { 1, 2, 3, 4 };
      Exception ex = new Exception();
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

      internal void Send(LogItem item)
      {
         calls++;
         lastItem = item;
      }

      internal abstract IInternalLogger CreateLogger();


      /// <summary>
      ///A test for All
      ///</summary>
      [TestMethod()]
      public void DetailedTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Detailed(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Detailed(message, id);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.All);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(id, lastItem.Ids[0]);
      }

      /// <summary>
      ///A test for All
      ///</summary>
      [TestMethod()]
      public void DetailedTest1()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Detailed(message, ids);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Detailed(message, ids);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(message, lastItem.Message);
         Assert.AreEqual(ids, lastItem.Ids);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Category, category);
      }

      /// <summary>
      ///A test for Always
      ///</summary>
      [TestMethod()]
      public void AlwaysTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Always(message);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Always(message);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Always);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
      }

      /// <summary>
      ///A test for Debug
      ///</summary>
      [TestMethod()]
      public void DebugTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Debug(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Debug(message, id);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.Debug);
         target.Debug(message, id);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Debug);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(id, lastItem.Ids[0]);
      }

      /// <summary>
      ///A test for Debug
      ///</summary>
      [TestMethod()]
      public void DebugTest1()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Debug(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Debug(message, ids);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Debug);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(ids, lastItem.Ids);

         calls = 0;
         target.SetLevel(LogLevel.Debug);
         target.Debug(message, id);
         Assert.AreEqual(calls, 1);
      }

      /// <summary>
      ///A test for Error
      ///</summary>
      [TestMethod()]
      public void ErrorTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Error(message, ex);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Error(message, ex);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.Error);
         target.Error(message, ex);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Error);
         Assert.IsNotNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Ex, ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
      }

      /// <summary>
      ///A test for Event
      ///</summary>
      [TestMethod()]
      public void EventTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Event(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Event(message, id);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.Event);
         target.Event(message, id);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Event);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(id, lastItem.Ids[0]);
      }

      /// <summary>
      ///A test for Event
      ///</summary>
      [TestMethod()]
      public void EventTest1()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Event(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Event(message, ids);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Event);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(ids, lastItem.Ids);

         calls = 0;
         target.SetLevel(LogLevel.Event);
         target.Event(message, id);
         Assert.AreEqual(calls, 1);
      }

      /// <summary>
      ///A test for Fatal
      ///</summary>
      [TestMethod()]
      public void FatalTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Fatal(message, ex);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Fatal(message, ex);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.Fatal);
         target.Fatal(message, ex);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Fatal);
         Assert.IsNotNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Ex, ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
      }

      /// <summary>
      ///A test for Info
      ///</summary>
      [TestMethod()]
      public void InfoTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Info(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Info(message, id);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.Info);
         target.Info(message, id);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Info);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(id, lastItem.Ids[0]);
      }

      /// <summary>
      ///A test for Info
      ///</summary>
      [TestMethod()]
      public void InfoTest1()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Info(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Info(message, ids);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Info);
         Assert.IsNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(ids, lastItem.Ids);

         calls = 0;
         target.SetLevel(LogLevel.Info);
         target.Info(message, id);
         Assert.AreEqual(calls, 1);
      }

      /// <summary>
      ///A test for Warning
      ///</summary>
      [TestMethod()]
      public void WarningTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Warning(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Warning(message, id);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.Warning);
         target.Warning(message, id, ex);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Warning);
         Assert.IsNotNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Ex, ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(id, lastItem.Ids[0]);
      }

      /// <summary>
      ///A test for Warning
      ///</summary>
      [TestMethod()]
      public void WarningTest1()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Warning(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Warning(message, ids, ex);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.Category, category);
         Assert.AreEqual(lastItem.Level, LogLevel.Warning);
         Assert.IsNotNull(lastItem.Ex);
         Assert.AreEqual(lastItem.Ex, ex);
         Assert.AreEqual(lastItem.Message, message);
         Assert.IsNotNull(lastItem.Ids);
         Assert.AreEqual(ids, lastItem.Ids);

         calls = 0;
         target.SetLevel(LogLevel.Warning);
         target.Warning(message, id);
         Assert.AreEqual(calls, 1);
      }


      /// <summary>
      ///A test for indexer
      ///</summary>
      [TestMethod()]
      public void ItemTest()
      {
         IInternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         target[LogLevel.Info] = true;

         calls = 0;
         target.Warning(message);
         target.Info(message);
         target.Debug(message);
         Assert.AreEqual(calls, 1);

         calls = 0;
         target.SetLevel(LogLevel.All);
         target[LogLevel.Info] = false;
         target.Warning(message);
         target.Info(message);
         target.Debug(message);

         Assert.AreEqual(calls, 2);
      }

   }
}
