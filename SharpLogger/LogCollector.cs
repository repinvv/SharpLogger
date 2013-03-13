using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SharpLogger.LogWrite;

namespace SharpLogger
{
    class LogCollector
    {
        IQueued<LogItem> qThread;
        HashSet<int> filter;
        ConcurrentQueue<Action<HashSet<int>>> filterQue;
        ILogWriter writer;

        public LogCollector(IQueued<LogItem> qThread, ILogWriter writer)
        {
            this.writer = writer;
            filterQue = new ConcurrentQueue<Action<HashSet<int>>>();
            filter = new HashSet<int>();
            this.qThread = qThread;
            qThread.SetTimeout(writer.GetTimeout());
            qThread.OnReceive += Receive;
            qThread.OnTimeout += TimeOut;
        }

        public void Send(LogItem message)
        {
            qThread.Send(message);
        }

        void RefreshIds()
        {
            while (!filterQue.IsEmpty)
            {
                Action<HashSet<int>> refresh;
                if (filterQue.TryDequeue(out refresh) && refresh != null)
                {
                    refresh(filter);
                }
            }
        }

        bool CheckIds(IEnumerable<int> ids)
        {
            if (!filterQue.IsEmpty)
            {
                RefreshIds();
            }
            return (filter.Count == 0
               || ids.FirstOrDefault(x => filter.Contains(x)) != default(int));
        }

        void Receive(LogItem message)
        {
            if (message.Level <= LogLevel.Error || CheckIds(message.Ids))
            {
                writer.Write(message);
            }
        }

        void TimeOut()
        {
            writer.Flush();
        }

        public void ShutDown()
        {
            qThread.Terminate();
        }

        public void FilterAddID(int id)
        {
            filterQue.Enqueue(x => x.Add(id));
        }

        public void FilterRemoveID(int id)
        {
            filterQue.Enqueue(x => x.Remove(id));
        }

        public void FilterClear()
        {
            filterQue.Enqueue(x => x.Clear());
        }
    }
}
