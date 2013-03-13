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

        TimeSpan forceFlush;
        int flushTimeout;
        int rotationSize;
        private string filename;
        private int rotationFiles;
        StringBuilder buffer;
        DateTime lastFlush;
        ILogConstructor constructor;

        public LogFileWriter(ILogConstructor constructor, IOptions options)
        {
            this.constructor = constructor;
            flushTimeout = options.GetInt("LogFileFlush", Timeout.Infinite);
            if (flushTimeout == 0)
            {
                flushTimeout = Timeout.Infinite;
            }
            forceFlush = TimeSpan.FromSeconds((double)flushTimeout / 500);
            filename = options.Get("LogFilename", "./logs.log");
            rotationSize = (1 << 20) * options.GetInt("LogRotationSizeMb", 5);
            rotationFiles = options.GetInt("LogRotationFiles", 5);
            var append = options["LogFileAppend"].ToLower() == "true";
            if (!append && File.Exists(filename))
            {
                Rotate();
            }
            buffer = new StringBuilder(BufferSize + 256);
            lastFlush = DateTime.Now;
        }

        public void Write(LogItem message)
        {
            TimeSpan elapsed = (DateTime.Now - lastFlush);
            constructor.ConstructLine(buffer, message);
            if (buffer.Length >= BufferSize
               || elapsed > forceFlush)
            {
                Flush();
            }
        }

        public void Flush()
        {
            lastFlush = DateTime.Now;
            var output = buffer.ToString();
            buffer.Clear();
            var outputBytes = Encoding.UTF8.GetBytes(output);
            if (File.Exists(filename))
            {
                var fi = new FileInfo(filename);
                if (fi.Length + outputBytes.Length > rotationSize)
                {
                    Rotate();
                }
            }
            File.AppendAllText(filename, output);
        }

        void Rotate()
        {
            try
            {
                string name1 = filename + "." + rotationFiles;
                if (File.Exists(name1))
                {
                    File.Delete(name1);
                }
                for (int n = rotationFiles - 1; n > 0; n--)
                {
                    string name2 = filename + "." + n;
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
            return flushTimeout;
        }
    }
}
