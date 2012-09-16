using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
  interface ILogConstructor
  {
    void ConstructLine(StringBuilder sb, LogItem item);
  }  
}
