using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace LoggerTest
{
    
    
    /// <summary>
    ///This is a test class for CLCIdAppenderTest and is intended
    ///to contain all CLCIdAppenderTest Unit Tests
    ///</summary>
   [TestClass()]
   public class CLCIdAppenderTest : LogConstructorTest
   {

      internal override ILogConstructor CreateLogConstructor()
      {
         var item = new CLCIdAppender(":");
         item.Chain(new CLCResultReturner());
         return item;
      }

      internal override bool stringCriteria(string constructed)
      {
         var split = constructed.Split(':');
         if (split.Length == 2)         
            if (ids[0].ToString() == split[0])
               if (ids[1].ToString() == split[1])
                  return true;
         return false;
      }
   }
}
