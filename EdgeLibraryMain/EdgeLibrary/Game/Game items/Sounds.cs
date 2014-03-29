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

namespace EdgeLibrary
{
    /// <summary>
    /// Stores all the SoundEffects/music and plays them
    /// </summary>
    public static class Sounds
    {
        private static Dictionary<string, SoundEffect> SoundEffects;
        private static Dictionary<string, Song> Songs;
        private static ContentManager Content;

        public static bool IsRepeating { get { return MediaPlayer.IsRepeating; } set { MediaPlayer.IsRepeating = value; } }
        public static bool IsMuted { get { return MediaPlayer.IsMuted; } set { MediaPlayer.IsMuted = value; } }
        public static float Volume { get { return MediaPlayer.Volume; } set { MediaPlayer.Volume = value; } }
        public static bool IsShuffled { get { return MediaPlayer.IsShuffled; } set { MediaPlayer.IsShuffled = value; } }

        public static void Init(ContentManager c)
        {
            SoundEffects = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();

            Content = c;
        }

        public static void LoadSound(string path)
        {
            addSound(MathTools.LastPortionOfPath(path), Content.Load<SoundEffect>(path));
        }

        public static void LoadSound(string path, string name)
        {
            addSound(name, Content.Load<SoundEffect>(path));
        }

        public static void LoadSong(string path)
        {
            addSong(MathTools.LastPortionOfPath(path), Content.Load<Song>(path));
        }

        public static void LoadSong(string path, string name)
        {
            addSong(name, Content.Load<Song>(path));
        }


        public static void addSound(string soundName, SoundEffect sound)
        {
            DebugLogger.LogAdd("Sound added. Name: " + soundName);
            SoundEffects.Add(soundName, sound);
        }

        public static void addSong(string songName, Song song)
        {
            DebugLogger.LogAdd("Song added. Name: " + songName);
            Songs.Add(songName, song);
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
            foreach (var song in Songs)
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
            foreach (var sound in SoundEffects)
            {
                if (sound.Key == soundName)
                {
                    return sound.Value;
                }
            }
            return null;
        }
    }
}
