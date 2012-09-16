using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLogger;

namespace LoggerTest
{

   [TestClass()]
   public class IQueuedConcurRegTest : IQueuedTest<TestMessageStruct>
   {
      protected override IQueued<TestMessageStruct> createSubject()
      {
         return new ConcurrentQueued<TestMessageStruct>(new RegularThreadStarter());
      }

      protected override TestMessageStruct CreateItem()
      {
         var item = new TestMessageStruct();
         var rnd = new Random();
         item.id = rnd.Next();
         item.message = string.Format("message {0}", item.id);
         return item;
      }
   }

   [TestClass()]
   public class IQueuedConcurRegTest2 : IQueuedTest<TestMessageClass>
   {
      protected override IQueued<TestMessageClass> createSubject()
      {
         return new ConcurrentQueued<TestMessageClass>(new RegularThreadStarter());
      }

      protected override TestMessageClass CreateItem()
      {
         var item = new TestMessageClass();
         var rnd = new Random();
         item.id = rnd.Next();
         item.message = string.Format("message {0}", item.id);
         return item;
      }
   }
}
