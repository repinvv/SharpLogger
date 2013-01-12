using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SharpOptions;
using System.IO;

namespace SharpLogger
{
    class LogFileWriter : ILogWriter
    {
        const int bufferSize = 2000000;

        TimeSpan forceFlush;
        int flushTimeout;
        int rotationSize;
        String[] fileName;
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
            string dirname = options["LogBaseDir"];
            dirname.Replace('\\', '/');
            if (dirname!=string.Empty && dirname.Last() != '/')
            {
                dirname += "/logs/";
            }
            else
            {
                dirname += "logs/";
            }
            Directory.CreateDirectory(dirname);
            rotationSize = (1 << 20) * options.GetInt("LogRotationSizeMb", 5);
            fileName = new string[options.GetInt("LogRotationFiles", 5)];
            fileName[0] = dirname + "latest.log";
            for (int n = 1; n < fileName.Length; n++)
                fileName[n] = dirname + "old" + n + ".log";
            bool append = options["LogFileAppend"].ToLower() == "true";
            if (!append)
            {
                if (File.Exists(fileName[0]))
                {
                    Rotate();
                }
            }
            buffer = new StringBuilder(bufferSize);
            lastFlush = DateTime.Now;
        }

        public void Write(LogItem message)
        {
            TimeSpan elapsed = (DateTime.Now - lastFlush);
            constructor.ConstructLine(buffer, message);
            if (buffer.Length >= bufferSize
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
            var outputBytes = UTF8Encoding.UTF8.GetBytes(output);
            var file = FileOpen();
            if (file == null)
            {
                return;
            }
            if (file.Length + outputBytes.Length > rotationSize)
            {
                file.Close();
                file.Dispose();
                Rotate();
                file = FileOpen();
            }
            if (file == null)
            {
                return;
            }
            file.Write(outputBytes, 0, outputBytes.Length);
            file.Close();
            file.Dispose();
        }

        FileStream FileOpen()
        {
            try
            {
                if (File.Exists(fileName[0]))
                {
                    return new FileStream(fileName[0], FileMode.Append, FileAccess.Write, FileShare.Read);
                }
                var fileStream = new FileStream(fileName[0], FileMode.Create, FileAccess.Write, FileShare.Read);
                var preamble = UTF8Encoding.UTF8.GetPreamble();
                fileStream.Write(preamble, 0, preamble.Length);
                return fileStream;
            }
            catch (Exception)
            {
                //fail silently
            }
            return null;
        }

        void Rotate()
        {
            try
            {
                if (File.Exists(fileName.Last()))
                    File.Delete(fileName.Last());
                for (int n = fileName.Length - 2; n >= 0; n--)
                    if (File.Exists(fileName[n]))
                        File.Move(fileName[n], fileName[n + 1]);
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
