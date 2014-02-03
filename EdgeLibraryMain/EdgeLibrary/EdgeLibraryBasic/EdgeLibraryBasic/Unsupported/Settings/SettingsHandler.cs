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
using System.Xml;

namespace EdgeLibrary
{
    public struct EdgeSetting
    {
        string settingName;
        string settingValue;

        public EdgeSetting(string name, string value)
        {
            settingName = name;
            settingValue = value;
        }

        public string Name
        {
            get { return settingName;  }
            set { if (value != null) { settingName = value; } }
        }

        public string Value
        {
            get { return settingValue;  }
            set { if (value != null) { settingValue = value; } }
        }
    }

    class SettingsHandler
    {
        List<EdgeSetting> settings;

        public SettingsHandler(List<EdgeSetting> settingsToAdd)
        {
            settings = new List<EdgeSetting>(settingsToAdd);
        }

        public SettingsHandler(params EdgeSetting[] settingsToAdd)
        {
            settings = new List<EdgeSetting>(settingsToAdd);
        }

        public SettingsHandler(string xmlPath)
        {
            reloadSettings(xmlPath);
        }

        public void reloadSettings(string xmlPath)
        {
            settings = new List<EdgeSetting>();

            XmlDocument settingsDocument = new XmlDocument();
            settingsDocument.Load(xmlPath);

            XmlNodeList settingXMLList = settingsDocument.ChildNodes[1].ChildNodes;

            foreach (XmlNode node in settingXMLList)
            {
                if (node.NodeType != XmlNodeType.Comment)
                {
                    settings.Add(new EdgeSetting(node.Name, node.InnerText));
                }
            }
        }

        public void setSetting(string settingToChange, string newValue)
        {
            for (int i = 0; i < settings.Count; i++ )
            {
                if (settings[i].Name == settingToChange)
                {
                    //settings[i].Value = newValue;
                }
            }
        }

        public void addSetting(string name, string value)
        {
            settings.Add(new EdgeSetting(name, value));
        }

        public string getSetting(string settingName)
        {
            foreach(EdgeSetting setting in settings)
            {
                if (setting.Name == settingName)
                {
                    return setting.Value;
                }
            }
            return null;
        }
    }
}
