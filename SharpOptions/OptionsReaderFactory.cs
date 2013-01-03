using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOptions
{
    public enum OptionsReaderType
    {
        Default,
        TextFile,
        XmlFile,
        Database
    }

    class OptionsReaderFactory
    {
        public OptionsReader CreateOptionsReader(OptionsReaderType type, string path, string name)
        {
            OptionsReader options;
            switch (type)
            {
                case OptionsReaderType.Default:
                    options = ReaderCheck(new OptionsXmlReader(path + name));
                    if (options is OptionsNullReader)
                    {
                        options = ReaderCheck(new OptionsTextReader(path + name));
                    }
                    break;
                case OptionsReaderType.TextFile:
                    options = ReaderCheck(new OptionsTextReader(path + name));
                    break;
                case OptionsReaderType.XmlFile:
                    options = ReaderCheck(new OptionsXmlReader(path + name));
                    break;
                case OptionsReaderType.Database:
                    options = ReaderCheck(new OptionsDatabaseReader(path, name));
                    break;
                default:
                    options = new OptionsNullReader();
                    break;
            }
            return options;

        }

        public OptionsReader ReaderCheck(OptionsReader reader)
        {
            if (reader.CanRead)
            {
                return reader;
            }
            else
            {
                return new OptionsNullReader();
            }
        }
    }
}
