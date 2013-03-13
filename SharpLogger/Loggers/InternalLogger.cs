namespace SharpLogger.Loggers
{
    interface IInternalLogger : Logger
    {
        void SetLevel(int level);
        void SetSender(Sender sender);

        bool this[int level] { set; }
    }
}
