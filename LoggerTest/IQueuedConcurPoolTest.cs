using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLogger;

namespace LoggerTest
{

   [TestClass()]
   public class IQueuedConcurPoolTest : IQueuedTest<TestMessageStruct>
   {
      protected override IQueued<TestMessageStruct> createSubject()
      {
         return new ConcurrentQueued<TestMessageStruct>(new PoolThreadStarter());
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
   public class IQueuedConcurPoolTest2 : IQueuedTest<TestMessageClass>
   {
      protected override IQueued<TestMessageClass> createSubject()
      {
         return new ConcurrentQueued<TestMessageClass>(new PoolThreadStarter());
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
