namespace SharpLogger.LogWrite
{
    interface ILogWriter
    {
        void Write(LogItem message);
        void Flush();
        int GetTimeout();
    }
}
