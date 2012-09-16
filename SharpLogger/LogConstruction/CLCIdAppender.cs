using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
  class CLCIdAppender : ConfigurableLogConstructor
  {
    char idSplitChar;
     
    public CLCIdAppender(string idSplit)
    {
       if (idSplit == string.Empty)
       {
          this.idSplitChar = ' ';
       }
       else
       {
          this.idSplitChar = idSplit[0];
       }
    }

    public override void ConstructLine(StringBuilder sb, LogItem item)
    {
      bool first = true;
      foreach (int id in item.ids)
      {
         if (id != default(int))
         {
            if (!first)
            {
               sb.Append(idSplitChar);
            }
            first = false;
            sb.Append(id.ToString());
         }
      }
      next.ConstructLine(sb, item);
    }
  }
}
