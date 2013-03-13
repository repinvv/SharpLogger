namespace SharpLogger
{
    public static class LogLevel
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
        private static string[] levels = new string[Total];

        internal static void SetLevel(int level, string levelString)
        {
            if (isLevelValid(level))
            {
                levels[level] = levelString;
            }
        }

        internal static string GetLevel(int level)
        {
            if (isLevelValid(level))
            {
                return levels[level];
            }
            return levels[_default];
        }

        internal static void SetDefault(string levelString)
        {
            for (int n = 0; n < levels.Length; n++)
            {
                if (levels[n] == levelString)
                {
                    _default = n;
                    return;
                }
            }
            int level;
            if (int.TryParse(levelString, out level))
            {
                Default = level;
            }
        }

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
