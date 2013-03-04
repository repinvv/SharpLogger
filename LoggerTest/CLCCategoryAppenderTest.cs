using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using SharpLogger.LogConstruction;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCCategoryAppenderTest and is intended
    ///to contain all CLCCategoryAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCCategoryAppenderTest : LogConstructorTest
   {

      internal override ILogConstructor CreateLogConstructor()
      {
         return new CategoryAppender();
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed == category;
      }
   }
}
