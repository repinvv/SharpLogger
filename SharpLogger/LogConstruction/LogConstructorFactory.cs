using System.Text;
using SharpOptions;

namespace SharpLogger.LogConstruction
{
    class LogConstructorFactory
    {
        IOptions _options;
        ConfigurableLogConstructor _constructor;
        StringBuilder _buffer = new StringBuilder();

        public LogConstructorFactory(IOptions options)
        {
            _options = options;
        }

        void Escaped(char val)
        {
            switch (val)
            {
                case 'd':
                    AddConstructor(
                       new DateTimeAppender(
                          _options["LogDateTimeFormat"],
                          _options["LogMilisecondsFormat"]));
                    break;
                case 'l':
                    AddConstructor(new LevelAppender());
                    break;
                case 'i':
                    AddConstructor(
                        new IDAppender(_options["LogIDSplitChar"]
                            , _options["LogIDBracketLeft"]
                            , _options["LogIDBracketRight"]));
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
                    _buffer.Append('\r');
                    break;
                case 'n':
                    _buffer.Append('\n');
                    break;
                default:
                    _buffer.Append(val);
                    break;
            }
        }

        void AddStringAppender()
        {
            if (_buffer.Length > 0)
            {
                var output = _buffer.ToString();
                _buffer.Clear();
                _constructor.Add(new StringAppender(output));
            }
        }

        void AddConstructor(ILogConstructor constructor)
        {
            AddStringAppender();
            _constructor.Add(constructor);
        }

        public ILogConstructor GetLogConstructor()
        {
            string messageFormat = _options["LogMessageFormat"];
            if (messageFormat == "basic" || messageFormat.Length < 5)
                return new BasicLogConstructor();
            _constructor = new ConfigurableLogConstructor();
            _buffer.Clear();
            for (int n = 0; n < messageFormat.Length; n++)
            {
                if (n < messageFormat.Length - 1 && messageFormat[n] == '\\')
                {
                    Escaped(messageFormat[n + 1]);
                    n++;
                }
                else
                {
                    _buffer.Append(messageFormat[n]);
                }
            }
            AddStringAppender();
            _constructor.Finished();
            return _constructor;
        }
    }
}
