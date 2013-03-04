using System.Text;

namespace SharpLogger.LogConstruction
{
    interface ILogConstructor
    {
        void ConstructLine(StringBuilder sb, LogItem item);
    }
}
