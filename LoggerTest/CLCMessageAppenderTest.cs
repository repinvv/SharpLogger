using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCMessageAppenderTest and is intended
    ///to contain all CLCMessageAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCMessageAppenderTest : LogConstructorTest
   {
      internal override ILogConstructor CreateLogConstructor()
      {
         var item = new CLCMessageAppender();
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         return constructed == message;
      }
   }
}
