/*
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
using System.Text;

namespace EdgeLibrary
{
    /// <summary>
    /// Logs all the events in the game - not really used
    /// </summary>
    public static partial class DebugLogger
    {
        //The writer to write to the files
        private static StreamWriter streamWriter;
        //A list of all the logs the game has
        public static List<string> Logs;

        //Initializes with the default directory - which is EdgeLibrary/Debug
        public static void Init()
        {
            //Generates the path for the Debug folder
            StringBuilder path = new StringBuilder(Environment.CurrentDirectory);
            path.Remove(path.Length - 47, 47);
            path.Append("Debug");
            Init(path.ToString());
        }
        //Initializes with a custom folder
        public static void Init(string writePath)
        {
            string path = writePath + "/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt";
            streamWriter = new StreamWriter(path);
            streamWriter.WriteLine("Logs for:" + DateTime.Now.ToString());
            streamWriter.WriteLine();

            Logs = new List<string>();
        }

        //Logs a game event to the text file
        public static void Log(string text, params string[] properties) { Log(' ', text, properties); }
        public static void Log(char modifier, string text, params string[] properties)
        {
            string log = text + " { ";
            foreach (string property in properties)
            {
                log += property + ", ";
            }
            log.Remove(log.Length - 1);
            log += " }";
            Logs.Add(log);

            if (streamWriter != null)
            {
                streamWriter.WriteLine(modifier + " " + text);
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

        //Logs an important game event to the text file - symbol: > <>
        public static void LogEvent(string text, params string[] properties)
        {
            string log = text + " { ";
            foreach (string property in properties)
            {
                log += property + ", ";
            }
            log.Remove(log.Length - 1);
            log += " }";
            Logs.Add(log);

            if (streamWriter != null)
            {
                for (int i = 0; i < text.Length / 2 + 4; i++)
                {
                    streamWriter.Write("<>");
                }
                streamWriter.WriteLine();
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
                for (int i = 0; i < text.Length / 2 + 4; i++)
                {
                    streamWriter.Write("<>");
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
            }
        }

        //Logs the addition of a new object to the game - symbol: +
        public static void LogAdd(string text, params string[] properties)
        {
            string log = text + " { ";
            foreach (string property in properties)
            {
                log += property + ", ";
            }
            log.Remove(log.Length - 1);
            log += " }";
            Logs.Add(log);

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

        //Logs the removal of a game object - symbol: -
        public static void LogRemove(string text, params string[] properties)
        {
            string log = text + " { ";
            foreach (string property in properties)
            {
                log += property + ", ";
            }
            log.Remove(log.Length - 1);
            log += " }";
            Logs.Add(log);

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

        //Logs a game warning - symbol: !
        public static void LogWarning(string text, params string[] properties)
        {
            string log = text + " { ";
            foreach (string property in properties)
            {
                log += property + ", ";
            }
            log.Remove(log.Length - 1);
            log += " }";
            Logs.Add(log);

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

        //Logs a serious game warning - symbol: ! ##
        public static void LogError(string text, params string[] properties)
        {
            string log = text + " { ";
            foreach (string property in properties)
            {
                log += property + ", ";
            }
            log.Remove(log.Length - 1);
            log += " }";
            Logs.Add(log);

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
 */
