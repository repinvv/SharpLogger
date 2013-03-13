using System.Text;
using SharpOptions;

namespace SharpLogger.LogConstruction
{
    class LogConstructorFactory
    {
        IOptions options;
        ConfigurableLogConstructor constructor;
        StringBuilder buffer = new StringBuilder();

        public LogConstructorFactory(IOptions options)
        {
            this.options = options;
        }

        void Escaped(char val)
        {
            switch (val)
            {
                case 'd':
                    AddConstructor(
                       new DateTimeAppender(
                          options["LogDateTimeFormat"],
                          options["LogMilisecondsFormat"]));
                    break;
                case 'l':
                    AddConstructor(new LevelAppender());
                    break;
                case 'i':
                    AddConstructor(
                        new IDAppender(options["LogIDSplitChar"]
                            , options["LogIDBracketLeft"]
                            , options["LogIDBracketRight"]));
                    break;
                case 'c':
                    AddConstructor(new CategoryAppender());
                    break;
                case 'm':
                    AddConstructor(new MessageAppender());
                    break;
                case 'e':
                    AddConstructor(new ExceptionAppender());
                    break;
                case 't':
                    AddConstructor(new StackTraceExceptionAppender());
                    break;
                case 'r':
                    buffer.Append('\r');
                    break;
                case 'n':
                    buffer.Append('\n');
                    break;
                default:
                    buffer.Append(val);
                    break;
            }
        }

        void AddStringAppender()
        {
            if (buffer.Length > 0)
            {
                var output = buffer.ToString();
                buffer.Clear();
                constructor.Add(new StringAppender(output));
            }
        }

        void AddConstructor(ILogConstructor constructor)
        {
            AddStringAppender();
            this.constructor.Add(constructor);
        }

        public ILogConstructor GetLogConstructor()
        {
            string messageFormat = options["LogMessageFormat"];
            if (messageFormat == "basic" || messageFormat.Length < 5)
                return new BasicLogConstructor();
            constructor = new ConfigurableLogConstructor();
            buffer.Clear();
            for (int n = 0; n < messageFormat.Length; n++)
            {
                if (n < messageFormat.Length - 1 && messageFormat[n] == '\\')
                {
                    Escaped(messageFormat[n + 1]);
                    n++;
                }
                else
                {
                    buffer.Append(messageFormat[n]);
                }
            }
            AddStringAppender();
            constructor.Finished();
            return constructor;
        }
    }
}
