using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
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

      internal abstract InternalLogger CreateLogger();


      /// <summary>
      ///A test for All
      ///</summary>
      [TestMethod()]
      public void DetailedTest()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Detailed(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Detailed(message, id);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.All);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(id, lastItem.ids[0]);
      }

      /// <summary>
      ///A test for All
      ///</summary>
      [TestMethod()]
      public void DetailedTest1()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Detailed(message, ids);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Detailed(message, ids);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(message, lastItem.message);
         Assert.AreEqual(ids, lastItem.ids);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.category, category);
      }

      /// <summary>
      ///A test for Always
      ///</summary>
      [TestMethod()]
      public void AlwaysTest()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Always(message);
         Assert.AreEqual(calls, 1);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Always(message);
         Assert.AreEqual(calls, 1);
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Always);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
      }

      /// <summary>
      ///A test for Debug
      ///</summary>
      [TestMethod()]
      public void DebugTest()
      {
         InternalLogger target = CreateLogger();
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
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Debug);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(id, lastItem.ids[0]);
      }

      /// <summary>
      ///A test for Debug
      ///</summary>
      [TestMethod()]
      public void DebugTest1()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Debug(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Debug(message, ids);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Debug);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(ids, lastItem.ids);

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
         InternalLogger target = CreateLogger();
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
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Error);
         Assert.IsNotNull(lastItem.ex);
         Assert.AreEqual(lastItem.ex, ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
      }

      /// <summary>
      ///A test for Event
      ///</summary>
      [TestMethod()]
      public void EventTest()
      {
         InternalLogger target = CreateLogger();
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
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Event);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(id, lastItem.ids[0]);
      }

      /// <summary>
      ///A test for Event
      ///</summary>
      [TestMethod()]
      public void EventTest1()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Event(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Event(message, ids);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Event);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(ids, lastItem.ids);

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
         InternalLogger target = CreateLogger();
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
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Fatal);
         Assert.IsNotNull(lastItem.ex);
         Assert.AreEqual(lastItem.ex, ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
      }

      /// <summary>
      ///A test for Info
      ///</summary>
      [TestMethod()]
      public void InfoTest()
      {
         InternalLogger target = CreateLogger();
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
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Info);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(id, lastItem.ids[0]);
      }

      /// <summary>
      ///A test for Info
      ///</summary>
      [TestMethod()]
      public void InfoTest1()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Info(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Info(message, ids);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Info);
         Assert.IsNull(lastItem.ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(ids, lastItem.ids);

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
         InternalLogger target = CreateLogger();
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
         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Warning);
         Assert.IsNotNull(lastItem.ex);
         Assert.AreEqual(lastItem.ex, ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(id, lastItem.ids[0]);
      }

      /// <summary>
      ///A test for Warning
      ///</summary>
      [TestMethod()]
      public void WarningTest1()
      {
         InternalLogger target = CreateLogger();
         target.SetLevel(LogLevel.Always);
         calls = 0;
         target.Warning(message, id);
         Assert.AreEqual(calls, 0);
         calls = 0;
         target.SetLevel(LogLevel.All);
         target.Warning(message, ids, ex);
         Assert.AreEqual(calls, 1);

         Assert.AreEqual(lastItem.category, category);
         Assert.AreEqual(lastItem.level, LogLevel.Warning);
         Assert.IsNotNull(lastItem.ex);
         Assert.AreEqual(lastItem.ex, ex);
         Assert.AreEqual(lastItem.message, message);
         Assert.IsNotNull(lastItem.ids);
         Assert.AreEqual(ids, lastItem.ids);

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
         InternalLogger target = CreateLogger();
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
