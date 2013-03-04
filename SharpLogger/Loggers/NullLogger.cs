using System;

namespace SharpLogger.Loggers
{
    public class NullLogger : ILogger
    {

        public void Always(string message)
        {            
        }

        public void Fatal(string message, Exception ex = null)
        {            
        }

        public void Error(string message, Exception ex = null)
        {            
        }

        public void Warning(string message, int id = 0, Exception ex = null)
        {            
        }

        public void Warning(string message, int[] id, Exception ex = null)
        {            
        }

        public void Warning(string message, Exception ex)
        {            
        }

        public void Info(string message, int id = 0)
        {            
        }

        public void Info(string message, int[] id)
        {            
        }

        public void Event(string message, int id = 0)
        {            
        }

        public void Event(string message, int[] id)
        {            
        }

        public void Debug(string message, int id = 0)
        {            
        }

        public void Debug(string message, int[] id)
        {            
        }

        public void Detailed(string message, int id = 0)
        {            
        }

        public void Detailed(string message, int[] id)
        {            
        }
    }
}
