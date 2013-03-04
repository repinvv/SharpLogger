using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SharpLogger.LogWrite;

namespace SharpLogger
{
    class LogCollector
    {
        IQueued<LogItem> _qThread;
        HashSet<int> _filter;
        ConcurrentQueue<Action<HashSet<int>>> _filterQue;
        ILogWriter _writer;

        public LogCollector(IQueued<LogItem> qThread, ILogWriter writer)
        {
            _writer = writer;
            _filterQue = new ConcurrentQueue<Action<HashSet<int>>>();
            _filter = new HashSet<int>();
            _qThread = qThread;
            qThread.SetTimeout(writer.GetTimeout());
            qThread.OnReceive += Receive;
            qThread.OnTimeout += TimeOut;
        }

        public void Send(LogItem message)
        {
            _qThread.Send(message);
        }

        void RefreshIds()
        {
            while (!_filterQue.IsEmpty)
            {
                Action<HashSet<int>> refresh;
                if (_filterQue.TryDequeue(out refresh) && refresh != null)
                {
                    refresh(_filter);
                }
            }
        }

        bool CheckIds(IEnumerable<int> ids)
        {
            if (!_filterQue.IsEmpty)
            {
                RefreshIds();
            }
            return (_filter.Count == 0
               || ids.FirstOrDefault(x => _filter.Contains(x)) != default(int));
        }

        void Receive(LogItem message)
        {
            if (message.Level <= LogLevel.Error || CheckIds(message.Ids))
            {
                _writer.Write(message);
            }
        }

        void TimeOut()
        {
            _writer.Flush();
        }

        public void ShutDown()
        {
            _qThread.Terminate();
        }

        public void FilterAddID(int id)
        {
            _filterQue.Enqueue(x => x.Add(id));
        }

        public void FilterRemoveID(int id)
        {
            _filterQue.Enqueue(x => x.Remove(id));
        }

        public void FilterClear()
        {
            _filterQue.Enqueue(x => x.Clear());
        }
    }
}
