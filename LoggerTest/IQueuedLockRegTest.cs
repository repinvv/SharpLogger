using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpLogger;

namespace LoggerTest
{
   public class TestMessageClass
   {
      public string message; //object reference field
      public int id;         //value field
   }

   public struct TestMessageStruct
   {
      public string message; //object reference field
      public int id;         //value field
      public override string ToString()
      {
         return message + " " + base.ToString();
      }
   }

   [TestClass()]
   public class IQueuedLockRegTest : IQueuedTest<TestMessageStruct>
   {
      protected override IQueued<TestMessageStruct> createSubject()
      {
         return new LockQueued<TestMessageStruct>(new RegularThreadStarter());
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
   public class IQueuedLockRegTest2 : IQueuedTest<TestMessageClass>
   {
      protected override IQueued<TestMessageClass> createSubject()
      {
         return new LockQueued<TestMessageClass>(new RegularThreadStarter());
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
