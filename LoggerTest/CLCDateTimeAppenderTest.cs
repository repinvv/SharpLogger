using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCDateTimeAppenderTest and is intended
    ///to contain all CLCDateTimeAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCDateTimeAppenderTest : LogConstructorTest
   {

      internal override ILogConstructor CreateLogConstructor()
      {
         var item = new CLCDateTimeAppender("dd-MM-yyyy HH.mm.ss", ":{0:000}");
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         var split = constructed.Split(new char[] { '-', '.', ':' });
         return split.Length > 5;
      }
   }
}
