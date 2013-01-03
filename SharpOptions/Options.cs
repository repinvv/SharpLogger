using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace SharpOptions
{
    public class Options : IOptions
    {
        private ConcurrentDictionary<string, string> options;
        private OptionsReader optionsAccess;

        public Options(string path, string name, OptionsReaderType type = OptionsReaderType.Default)
        {
            options = new ConcurrentDictionary<string, string>();
            if (type != OptionsReaderType.Database && string.IsNullOrEmpty(path))
            {
                    path = Directory.GetCurrentDirectory();                
            }
            optionsAccess = new OptionsReaderFactory().CreateOptionsReader(type, path, name);
            ReadOptions();
        }

        void ReadOptions()
        {
            var optionPairs = optionsAccess.ReadOptions();
            foreach (var option in optionPairs)
                options.
                   AddOrUpdate(option.Key, option.Value, (k, v) =>
                   {
                       return (v != null && v != string.Empty)
                          ? v
                          : option.Value;
                   });
        }

        public string this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                options.AddOrUpdate(key, value, (k, v) => value);
            }
        }

        public bool TryAdd(string key, string value)
        {
            return options.TryAdd(key, value);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            int output = defaultValue;
            string optionValue;
            if (options.TryGetValue(key, out optionValue))
            {
                if (!int.TryParse(optionValue, out output))
                {
                    return defaultValue;
                }
            }
            return output;
        }

        public void Save()
        {
            optionsAccess.WriteOptions(options);
        }

        public string Get(string key, string defaultValue = null) //can't put string.empty here;
        {
            string val;
            options.TryGetValue(key, out val);
            return val ?? defaultValue ?? string.Empty;
        }
    }
}
