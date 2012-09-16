using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for LogConstructorTest and is intended
    ///to contain all LogConstructorTest Unit Tests
    ///</summary>
   [TestClass()]
   public abstract class LogConstructorTest
   {
      protected string category = "cat";
      protected int level = LogLevel.Error;
      protected string message = "message";
      protected int[] ids = new int[] { 3, 4, 0 };
      protected string exception = "exception";
      
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


      internal abstract ILogConstructor CreateLogConstructor();

      internal abstract bool stringCriteria(string constructed);

      /// <summary>
      ///A test for ConstructLine
      ///</summary>
      [TestMethod()]
      public void ConstructLineTest()
      {
         ILogConstructor target = CreateLogConstructor(); // TODO: Initialize to an appropriate value
         Exception ex = null;
         try
         {
            throw new Exception(exception);
         }
         catch (Exception ex1)
         {
            ex = ex1;
         }

         LogItem item = new LogItem(category, level, message, ids, ex);

         string actual;
         StringBuilder sb = new StringBuilder();
         target.ConstructLine(sb, item);
         actual = sb.ToString();
         Assert.IsTrue(stringCriteria(actual));
      }
   }
}
