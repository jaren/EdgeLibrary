using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace EdgeLibrary
{
    public static class DebugWriter
    {
        private static StreamWriter streamWriter;

        public static void Init(string writePath)
        {
            string path = writePath + "/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt";
            streamWriter = new StreamWriter(path);

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
