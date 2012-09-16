using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCExceptionAppenderTest and is intended
    ///to contain all CLCExceptionAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCExceptionAppenderTest : LogConstructorTest
   {

      internal override ILogConstructor CreateLogConstructor()
      {
         var item = new CLCExceptionAppender();
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed.Contains(exception);
      }
   }
}
