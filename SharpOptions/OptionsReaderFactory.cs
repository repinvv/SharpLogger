using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOptions
{
  class OptionsReaderFactory
  {
     public OptionsReader CreateOptionsAccess(string loadPath)
    {
      OptionsReader options;
      options = new OptionsReaderXml(loadPath);
      if (options.CanRead)
      {
         return options;
      }
      return new OptionsReaderText(loadPath);
    }
  }
}
