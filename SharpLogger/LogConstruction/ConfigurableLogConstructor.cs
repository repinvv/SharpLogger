using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    abstract class ConfigurableLogConstructor : ILogConstructor
    {
        protected ConfigurableLogConstructor next;

        public void Chain(ConfigurableLogConstructor next)
        {
            this.next = next;
        }

        public abstract void ConstructLine(StringBuilder sb, LogItem item);
    }
}
