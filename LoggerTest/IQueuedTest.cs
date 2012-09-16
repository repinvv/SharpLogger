using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
namespace LoggerTest
{
        
    /// <summary>
    ///This is a test class for IQueuedTest and is intended
    ///to contain all IQueuedTest Unit Tests
    ///</summary>
   [TestClass()]
   public abstract class IQueuedTest<T>
   {
      public const int SleepDelay = 50;
      public const int SleepDelay2 = 100;
      public const int SleepDelay3 = 200;

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
      class Tester<T1>
      {
         public volatile int timedCount = 0;
         public volatile int receivedCount = 0;
         public T1 lastItem;
         public volatile int delay = 0;

         public Tester(IQueued<T1> tested)
         {
            tested.OnReceive += Received;
            tested.OnTimeout += TimeOut;
         }

         void TimeOut()
         {
            timedCount++;
         }

         void Received(T1 item)
         {
            Thread.Sleep(delay);
            receivedCount++;
            lastItem = item;            
         }
      }      

      protected abstract IQueued<T> createSubject();

      protected abstract T CreateItem();

      void ActivationTestHelper(Tester<T> tester, IQueued<T> tested)
      {
         int retry = 30;
         while (!tested.Active && retry > 0)
         {
            retry--;
            Thread.Sleep(SleepDelay);
         }
         Assert.AreEqual(true, tested.Active);
      }

      [TestMethod()]
      public void SendTest()
      {
         var subject = createSubject();
         var tester = new Tester<T>(subject);
         ActivationTestHelper(tester, subject);
         T item = CreateItem();
         tester.delay = SleepDelay;
         subject.Send(item);
         Assert.AreEqual(0, tester.receivedCount);
         Thread.Sleep(SleepDelay2);
         Assert.AreEqual(1, tester.receivedCount);
         Assert.IsTrue(tester.lastItem.Equals(item));
         subject.Terminate();
      }

      [TestMethod()]
      public void TimeoutTest()
      {
         var subject = createSubject();
         var tester = new Tester<T>(subject);
         subject.SetTimeout(SleepDelay);
         ActivationTestHelper(tester, subject);
         T item = CreateItem();
         subject.Send(item);
         Assert.AreEqual(0, tester.timedCount);
         Thread.Sleep(SleepDelay3);
         Assert.AreEqual(1, tester.timedCount);
         subject.Terminate();
      }

      [TestMethod()]
      public void TerminateTest()
      {
         var subject = createSubject();
         var tester = new Tester<T>(subject);
         ActivationTestHelper(tester, subject);
         T item = CreateItem();
         subject.Terminate();         
         subject.Send(item);
         Thread.Sleep(SleepDelay3);
         Assert.AreEqual(0, tester.receivedCount);
         Assert.AreEqual(0, tester.timedCount);
      }
   }
}
