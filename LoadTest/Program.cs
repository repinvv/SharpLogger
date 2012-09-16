using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpLogger;
using System.Threading;
using System.Diagnostics;
using NLog;

namespace ManualTest
{

   class Program
   {
      static int threadCount = 3;
      static int messageCount = 20000;
      static ManualResetEvent startEvent = new ManualResetEvent(false);
      static ManualResetEvent[] finishEvents;

      static void CountFlood(object otid)
      {
         int tid = (int)otid;
         startEvent.WaitOne();
         var log = SharpLogger.LogAccess.GetLogger("flooder " + tid);
         foreach (var x in Enumerable.Range(0, messageCount))
         {
            log.Info("Message", x);
            if (x % 1000 == 0)
               Thread.Sleep(0);
         }
         finishEvents[tid].Set();
      }

      static void IdFlood(object otid)
      {
         int tid = (int)otid;
         startEvent.WaitOne();
         var log = SharpLogger.LogAccess.GetLogger("flooder " + tid);
         foreach (var x in Enumerable.Range(0, messageCount))
         {
            log.Info("Message", tid);
            if (x % 1000 == 0)
               Thread.Sleep(0);
         }
         finishEvents[tid].Set();
      }

      static void NlogFlood(object otid)
      {
         int tid = (int)otid;
         startEvent.WaitOne();
         var log = NLog.LogManager.GetLogger("flooder " + tid);
         foreach (var x in Enumerable.Range(0, messageCount))
         {
            log.Info("Message");
            if (x % 1000 == 0)
               Thread.Sleep(0);
         }
         finishEvents[tid].Set();
      }

      static int runThreads(Action<object> action)
      {
         Thread[] threads = new Thread[threadCount];
         Stopwatch sw = new Stopwatch();
         startEvent.Reset();
         finishEvents = new ManualResetEvent[threadCount];
         foreach (var n in Enumerable.Range(0, threadCount))
         {
            threads[n] = new Thread(new ParameterizedThreadStart(action));
            threads[n].Start(n);
            finishEvents[n] = new ManualResetEvent(false);
         }
         Thread.Sleep(500);
         
         sw.Start();
         startEvent.Set();
         WaitHandle.WaitAll(finishEvents);
         sw.Stop();
         foreach (var n in Enumerable.Range(0, threadCount))
         {
            threads[n].Join();
         }
         
         return (int)sw.ElapsedMilliseconds;
      }



      static void Main(string[] args)
      {
         LogAccess.GetLogger("main");

         int parse;
         if (args.Length >= 2)
         {
            if (int.TryParse(args[0], out parse))
            {
               if (parse > 0)
               {
                  threadCount = parse;
               }
            }
            if (int.TryParse(args[1], out parse))
            {
               if (parse > 0)
               {
                  messageCount = parse;
               }
            }
            
         }

         Console.WriteLine("Regular test");
         Console.WriteLine("Elapsed {0} miliseconds\n", runThreads(CountFlood));
         

         if (args.Length < 3)
         { 
            Thread.Sleep(5000);

         //Console.WriteLine("ID test");
         //LogAccess.FilterAddID(3);
         //Console.WriteLine("Elapsed {0} miliseconds\n", runThreads(IdFlood));

            Console.WriteLine("Nlog test");
            Console.WriteLine("Elapsed {0} miliseconds\n", runThreads(NlogFlood));
         }
         Console.ReadKey();
      }
   }

}
