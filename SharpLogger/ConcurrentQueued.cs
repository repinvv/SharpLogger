using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
namespace SharpLogger
{
   class ConcurrentQueued<T> : IQueued<T>
   {
      bool active = false;
      int timeout = Timeout.Infinite;
      ConcurrentQueue<T> queue = new ConcurrentQueue<T>();
      EventWaitHandle[] events = new EventWaitHandle[2];

      public event ReceiveHandler<T> OnReceive; 
      public event Action OnTimeout;

      public ConcurrentQueued(IThreadStarter starter, ManualResetEvent terminateEvent = null)
      {         
         events[0] = new AutoResetEvent(false);
         events[1] = terminateEvent ?? new ManualResetEvent(false);
         starter.Start(ThreadRun);
      }

      public bool Active
      {
         get { return active; }
      }      

      public void SetTimeout(int timeout)
      {
         this.timeout = timeout;
      }      

      public void Send(T msg)
      {
         if (!active)
         {
            return;
         }
         try
         {
            queue.Enqueue(msg);
         }
         catch (Exception)
         {
            //fail silently
         }
         events[0].Set();
      }

      public void Terminate()
      {
         events[1].Set();
      }

      void ThreadRun()
      {
         T item;
         int index;
         int currentTimeout = Timeout.Infinite;
         active = true;
         try
         {
            while ((index = WaitHandle.WaitAny(events, currentTimeout)) != 1)
            {
               if (index == WaitHandle.WaitTimeout)
               {
                  if (OnTimeout != null)
                  {
                     OnTimeout();
                  }
                  currentTimeout = Timeout.Infinite;
               }
               else
               {
                  if (queue.Count != 0)
                  {
                     while (queue.TryDequeue(out item))
                     {
                        if (OnReceive != null)
                        {
                           OnReceive(item);
                        }
                     }
                  }
                  currentTimeout = timeout;
               }

            }
            //terminated
            active = false;
            while (queue.TryDequeue(out item)) ;
         }
         catch (Exception ex)
         {
            var file = new FileStream("logfail", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            var bytes = UTF8Encoding.UTF8.GetBytes(ex.Message+"\n"+ex.StackTrace);
            file.Write(bytes, 0, bytes.Length);
            file.Close();
         }

      }
   }
}
