using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class CLCIdAppender : ConfigurableLogConstructor
    {
        string idSplit;
        string bracketLeft;
        string bracketRight;

        public CLCIdAppender(string idSplit, string bracketLeft, string bracketRight)
        {
            this.bracketLeft = bracketLeft;
            this.bracketRight = bracketRight;
            this.idSplit = string.IsNullOrEmpty(idSplit) 
                ? " " 
                : idSplit.Substring(0,1);
            
        }

        public override void ConstructLine(StringBuilder sb, LogItem item)
        {
            bool first = true;
            foreach (int id in item.ids)
            {
                if (id != default(int))
                {
                    sb.Append(first? bracketLeft : idSplit);                    
                    first = false;
                    sb.Append(id.ToString());
                }
            }
            if (!first)
            {
                sb.Append(bracketRight);
            }
            next.ConstructLine(sb, item);
        }
    }
}
