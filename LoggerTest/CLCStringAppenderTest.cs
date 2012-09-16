using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCStringAppenderTest and is intended
    ///to contain all CLCStringAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCStringAppenderTest : LogConstructorTest
   {
      string staticString = "staticString";


      internal override ILogConstructor CreateLogConstructor()
      {
         var item = new CLCStringAppender(staticString);
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed == staticString;
      }
   }
}
