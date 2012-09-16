using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

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
         var item = new CLCLevelAppender();
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed.Length > 3;
      }
   }
}
