using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SharpOptions
{
   class OptionsReaderXml : OptionsReader
   {
      const string Option = "option";
      const string Name = "name";
      const string Value = "value";
      
      string filename;
      XmlDocument doc;

      public OptionsReaderXml(string loadPath)
      {
         filename = loadPath+"cfg.xml";
      }

      public bool CanRead
      {
         get
         {
            doc = new XmlDocument();
            try
            {
               doc.Load(filename);
            }
            catch (Exception)
            {
               return false;
            }
            return true;
         }
      }

      string GetAttributeValue(XmlAttribute attribute)
      {
         if (attribute == null)
            return string.Empty;
         if (attribute.Value == null)
            return string.Empty;
         return attribute.Value;
      }

      public IEnumerable<KeyValuePair<string, string>> ReadOptions()
      {
         var list = new List<KeyValuePair<string,string>>();
         try
         {
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
               try
               {
                  if (node.Name == Option)
                  {
                     var optionName = GetAttributeValue(node.Attributes[Name]);
                     var optionValue = GetAttributeValue(node.Attributes[Value]);
                     if (optionName != string.Empty)
                        list.Add(new KeyValuePair<string, string>(optionName, optionValue));
                  }
               }
               catch (Exception)
               {
                  //fail silently
               }
            }
         }
         catch (Exception)
         {
            //fail silently
         }
         return list;
      }

      public void WriteOptions(IEnumerable<KeyValuePair<string, string>> options)
      {
         throw new NotImplementedException();
      }
   }
}
