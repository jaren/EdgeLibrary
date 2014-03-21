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
            streamWriter.WriteLine(DateTime.Now.ToString());
        }

        public static void Log(string text)
        {
            streamWriter.WriteLine(text);
        }
    }
}
