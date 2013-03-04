using System.Collections.Generic;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class ConfigurableLogConstructor : ILogConstructor
    {
        private List<ILogConstructor> _constructList = new List<ILogConstructor>();

        ILogConstructor[] _constructors = new ILogConstructor[0];

        public void Add(ILogConstructor next)
        {
            _constructList.Add(next);
        }

        public void Finished()
        {
            _constructors = _constructList.ToArray();
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            foreach (var logConstructor in _constructors)
            {
                logConstructor.ConstructLine(sb, item);
            }
        }
    }
}
