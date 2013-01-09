using SharpLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using SharpOptions;

namespace LoggerTest
{

    class OptionsMock : IOptions
    {
        public string Get(string key, string defaultValue = null)
        {
            throw new NotImplementedException();
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public string this[string key]
        {
            get
            {
                return "abcd";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool TryAdd(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
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
