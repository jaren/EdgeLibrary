using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EdgeLibrary
{
    public static class DebugWriter
    {
        private static StreamWriter streamWriter;

        public static void Init(string writePath)
        {
            streamWriter = new StreamWriter(writePath);
            streamWriter.WriteLine("Logs for:" + DateTime.Now.ToString());
            streamWriter.WriteLine();
        }

        public static void Log(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine(text);
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("     {");
                }
                foreach(string property in properties)
                {
                    streamWriter.WriteLine("        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("     }");
                }
                streamWriter.WriteLine();
            }
        }

        public static void LogEvent(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("> " + text);
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine(">    {");
                }
                foreach (string property in properties)
                {
                    streamWriter.WriteLine(">        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine(">    }");
                }
                streamWriter.WriteLine();
            }
        }

        public static void LogAdd(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("+ " + text);
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("+    {");
                }
                foreach (string property in properties)
                {
                    streamWriter.WriteLine("+        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("+    }");
                }
                streamWriter.WriteLine();
            }
        }

        public static void LogAddNSP(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("+ " + text);
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("+    {");
                }
                foreach (string property in properties)
                {
                    streamWriter.WriteLine("+        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("+    }");
                }
            }
        }


        public static void LogRemove(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("- " + text);
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("-    {");
                }
                foreach (string property in properties)
                {
                    streamWriter.WriteLine("-        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("-    }");
                }
                streamWriter.WriteLine();
            }
        }

        public static void LogWarning(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("! " + text);
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("!    {");
                }
                foreach (string property in properties)
                {
                    streamWriter.WriteLine("!        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("!    }");
                }
                streamWriter.WriteLine();
            }
        }

        public static void LogError(string text, params string[] properties)
        {
            if (streamWriter != null)
            {
                for (int i = 0; i < text.Length + 8; i++)
                {
                    streamWriter.Write('#');
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine("!!! " + text + " !!!");
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("#    {");
                }
                foreach (string property in properties)
                {
                    streamWriter.WriteLine("#        " + property);
                }
                if (properties.Length > 0)
                {
                    streamWriter.WriteLine("#    }");
                }
                for (int i = 0; i < text.Length + 8; i++)
                {
                    streamWriter.Write('#');
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
            }
        }
    }
}
