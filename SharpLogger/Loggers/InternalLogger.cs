namespace SharpLogger.Loggers
{
    interface IInternalLogger : ILogger
    {
        void SetLevel(int level);
        void SetSender(Sender sender);

        bool this[int level] { set; }
    }
}
