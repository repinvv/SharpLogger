using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    public class LogLevel
    {
        public const int Invalid = 0;
        public const int Always = 1;
        public const int Fatal = 2;
        public const int Error = 3;
        public const int Warning = 4;
        public const int Info = 5;
        public const int Event = 6;
        public const int Debug = 7;
        public const int All = 8;
        public const int Total = 9;
        private static int _default = Info;

        internal static int Default
        {
            get
            {
                return _default;
            }
            set
            {
                if (isLevelValid(value))
                {
                    _default = value;
                }
            }
        }

        internal static bool isLevelValid(int level)
        {
            return (level > Invalid && level < Total);
        }

    }
}
