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
    public static class DebugLogger
    {
        private static StreamWriter streamWriter;
        public static List<string> Logs;

        public static void Init(string writePath)
        {
            string path = writePath + "/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "." + DateTime.Now.Minute + "." + DateTime.Now.Second + ".txt";
            try
            {
                streamWriter = new StreamWriter(path);
                streamWriter.WriteLine("Logs for:" + DateTime.Now.ToString());
                streamWriter.WriteLine();
            }
            catch { }

            Logs = new List<string>();
        }

        public static void Log(string text, params string[] properties)
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
                for (int i = 0; i < text.Length/2 + 4; i++)
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
                for (int i = 0; i < text.Length/2 + 4; i++)
                {
                    streamWriter.Write("<>");
                }
                streamWriter.WriteLine();
                streamWriter.WriteLine();
            }
        }

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

    public class LoggerPanel : Element
    {

        public SpriteFont Font { get { return _font; } set { _font = value; reloadInfo(); } }
        public Color DrawColor { get { return _drawColor; } set { _drawColor = value; reloadInfo(); } }
        private SpriteFont _font;
        private Color _drawColor;

        private List<TextSprite> CommandSprites;
        public int CommandsToDisplay { get { return _commandsToDisplay; } set { _commandsToDisplay = value; reloadCommands(); } }
        private int _commandsToDisplay;

        public LoggerPanel(string fontName, Color drawColor, Vector2 position) : base(MathTools.RandomID(typeof(LoggerPanel)))
        {
            _commandsToDisplay = 5;
            CommandSprites = new List<TextSprite>();
            Font = ResourceManager.getFont(fontName);
            DrawColor = drawColor;
            reloadCommands();
        }

        private void reloadCommands()
        {
            float YDifference = _font.MeasureString(DebugLogger.Logs[0]).Y * 1.5f;

            CommandSprites.Clear();
            for (int i = 0; i < _commandsToDisplay; i++)
            {
                TextSprite CommandSprite = new TextSprite(string.Format("{0}_CommandSprite{1}", ID, i), "", "last Log:", Vector2.Zero, _drawColor);
                CommandSprite.Font = _font;
                CommandSprite.CenterAsOrigin = false;
                CommandSprite.Position = new Vector2(Position.X, Position.Y + YDifference * i);
                CommandSprites.Add(CommandSprite);
            }
        }

        private void reloadInfo()
        {
            foreach (TextSprite sprite in CommandSprites)
            {
                sprite.Font = _font;
                sprite.Style.Color = _drawColor;
            }
        }

        protected override void updateElement(GameTime gameTime)
        {
            for (int i = 0; i < CommandSprites.Count; i++)
            {
                if (DebugLogger.Logs.Count >= i)
                {
                    CommandSprites[i].Text = DebugLogger.Logs[DebugLogger.Logs.Count - i - 1];
                }
            }
        }

        protected override void drawElement(GameTime gameTime)
        {
            foreach (TextSprite sprite in CommandSprites)
            {
                sprite.Draw(gameTime);
            }
        }
    }
}
