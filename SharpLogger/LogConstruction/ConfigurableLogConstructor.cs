using System.Collections.Generic;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class ConfigurableLogConstructor : ILogConstructor
    {
        private List<ILogConstructor> constructList = new List<ILogConstructor>();

        ILogConstructor[] constructors = new ILogConstructor[0];

        public void Add(ILogConstructor next)
        {
            constructList.Add(next);
        }

        public void Finished()
        {
            constructors = constructList.ToArray();
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            foreach (var logConstructor in constructors)
            {
                logConstructor.ConstructLine(sb, item);
            }
        }
    }
}
