using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLogger;

namespace LoggerTest
{

   [TestClass()]
   public class IQueuedLockPoolTest : IQueuedTest<TestMessageStruct>
   {
      protected override IQueued<TestMessageStruct> createSubject()
      {
         return new LockQueued<TestMessageStruct>(new PoolThreadStarter());
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
   public class IQueuedLockPoolTest2 : IQueuedTest<TestMessageClass>
   {
      protected override IQueued<TestMessageClass> createSubject()
      {
         return new LockQueued<TestMessageClass>(new PoolThreadStarter());
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
