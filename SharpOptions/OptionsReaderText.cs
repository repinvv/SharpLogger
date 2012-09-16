using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpOptions
{
   class OptionsReaderText : OptionsReader
   {
      const int BufSize = 65536;
      string[] LineSeparators = new string[] { "\r\n", "\n" };
      char OptionSeparator = '=';

      string filename;

      public OptionsReaderText(string loadPath)
      {
         filename = loadPath + ".cfg";
      }

      public bool CanRead
      {
         get
         {
            return File.Exists(filename); //ok, it is oversimplified
         }
      }
      
      string ReadFile()
      {
         var sb = new StringBuilder();
         try
         {
            var file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            var reader = new StreamReader(file);
            char[] buf = new char[BufSize];
            int len = 1;
            while ((len = reader.Read(buf, 0, BufSize)) > 0)
            {
               sb.Append(buf, 0, len);
            }
            reader.Dispose();
            file.Close();
         }
         catch (Exception)
         {
            //fail silently
         }
         return sb.ToString();
      }

      void ParseOptions(string fileContents, List<KeyValuePair<string, string>> options)
      {
         var lines = fileContents.Split(LineSeparators, StringSplitOptions.RemoveEmptyEntries);
         foreach (var line in lines)
         {
            int separator = line.IndexOf(OptionSeparator);
            if (separator > 0 && separator < line.Length - 1)
            {
               string name = line.Substring(0, separator);
               string value = line.Substring(separator + 1);

               options.Add(new KeyValuePair<string, string>(name, value));
            }
         }
      }

      public void WriteOptions(IEnumerable<KeyValuePair<string, string>> options)
      {
         throw new NotImplementedException();
      }


      public IEnumerable<KeyValuePair<string, string>> ReadOptions()
      {
         var options = new List<KeyValuePair<string, string>>();
         ParseOptions(ReadFile(), options);
         return options;
      }
   }
}
