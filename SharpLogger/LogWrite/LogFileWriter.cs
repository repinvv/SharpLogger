using System;
using System.Text;
using System.Threading;
using SharpLogger.LogConstruction;
using SharpOptions;
using System.IO;

namespace SharpLogger.LogWrite
{
    class LogFileWriter : ILogWriter
    {
        const int BufferSize = 2000000;

        TimeSpan _forceFlush;
        int _flushTimeout;
        int _rotationSize;
        private string _filename;
        private int _rotationFiles;
        StringBuilder _buffer;
        DateTime _lastFlush;
        ILogConstructor _constructor;

        public LogFileWriter(ILogConstructor constructor, IOptions options)
        {
            _constructor = constructor;
            _flushTimeout = options.GetInt("LogFileFlush", Timeout.Infinite);
            if (_flushTimeout == 0)
            {
                _flushTimeout = Timeout.Infinite;
            }
            _forceFlush = TimeSpan.FromSeconds((double)_flushTimeout / 500);
            _filename = options.Get("LogFilename", "./logs.log");
            _rotationSize = (1 << 20) * options.GetInt("LogRotationSizeMb", 5);
            _rotationFiles = options.GetInt("LogRotationFiles", 5);
            var append = options["LogFileAppend"].ToLower() == "true";
            if (!append && File.Exists(_filename))
            {
                Rotate();
            }
            _buffer = new StringBuilder(BufferSize + 256);
            _lastFlush = DateTime.Now;
        }

        public void Write(LogItem message)
        {
            TimeSpan elapsed = (DateTime.Now - _lastFlush);
            _constructor.ConstructLine(_buffer, message);
            if (_buffer.Length >= BufferSize
               || elapsed > _forceFlush)
            {
                Flush();
            }
        }

        public void Flush()
        {
            _lastFlush = DateTime.Now;
            var output = _buffer.ToString();
            _buffer.Clear();
            var outputBytes = Encoding.UTF8.GetBytes(output);
            if (File.Exists(_filename))
            {
                var fi = new FileInfo(_filename);
                if (fi.Length + outputBytes.Length > _rotationSize)
                {
                    Rotate();
                }
            }
            File.AppendAllText(_filename, output);
        }

        void Rotate()
        {
            try
            {
                string name1 = _filename + "." + _rotationFiles;
                if (File.Exists(name1))
                {
                    File.Delete(name1);
                }
                for (int n = _rotationFiles - 1; n > 0; n--)
                {
                    string name2 = _filename + "." + n;
                    if (File.Exists(name2))
                    {
                        File.Move(name2, name1);
                    }
                    name1 = name2;
                }
            }
            catch (Exception)
            {
                //fail silently
            }
        }

        public int GetTimeout()
        {
            return _flushTimeout;
        }
    }
}
