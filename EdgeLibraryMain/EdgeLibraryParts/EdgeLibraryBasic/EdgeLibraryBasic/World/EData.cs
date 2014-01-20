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

namespace EdgeLibrary.Basic
{
    public static class EData
    {
        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, SpriteFont> fonts;
        private static Dictionary<string, SoundEffect> sounds;
        private static Dictionary<string, Song> songs;

        public static void Init()
        {
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            sounds = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();
        }

        public static void addTexture(string textureName, Texture2D texture)
        {
            textures.Add(textureName, texture);
        }

        public static void addFont(string fontName, SpriteFont font)
        {
            fonts.Add(fontName, font);
        }

        public static void addSound(string soundName, SoundEffect sound)
        {
            sounds.Add(soundName, sound);
        }

        public static void addSong(string songName, Song song)
        {
            songs.Add(songName, song);
        }

        public static void playSong(string songName)
        {
            MediaPlayer.Play(getSong(songName));
        }

        public static void playSound(string soundName)
        {
            getSound(soundName).Play();
        }

        public static Song getSong(string songName)
        {
            foreach (var song in songs)
            {
                if (song.Key == songName)
                {
                    return song.Value;
                }
            }
            return null;
        }

        public static SoundEffect getSound(string soundName)
        {
            foreach (var sound in sounds)
            {
                if (sound.Key == soundName)
                {
                    return sound.Value;
                }
            }
            return null;
        }

        public static Texture2D getTexture(string textureName)
        {
            foreach (var texture in textures)
            {
                if (texture.Key == textureName)
                {
                    return texture.Value;
                }
            }
            return null;
        }
        public static SpriteFont getFont(string fontName)
        {
            foreach (var font in fonts)
            {
                if (font.Key == fontName)
                {
                    return font.Value;
                }
            }
            return null;
        }
    }
}
