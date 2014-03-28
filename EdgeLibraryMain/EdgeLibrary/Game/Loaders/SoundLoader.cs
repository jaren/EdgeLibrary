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
    /// Stores all the Sounds/music and plays them
    /// </summary>
    public class SoundLoader
    {
        private Dictionary<string, SoundEffect> Sounds;
        private Dictionary<string, Song> Songs;
        private ContentManager Content;

        public bool IsRepeating { get { return MediaPlayer.IsRepeating; } set { MediaPlayer.IsRepeating = value; } }
        public bool IsMuted { get { return MediaPlayer.IsMuted; } set { MediaPlayer.IsMuted = value; } }
        public float Volume { get { return MediaPlayer.Volume; } set { MediaPlayer.Volume = value; } }
        public bool IsShuffled { get { return MediaPlayer.IsShuffled; } set { MediaPlayer.IsShuffled = value; } }

        public SoundLoader(ContentManager c)
        {
            Sounds = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();

            Content = c;
        }

        public void LoadSound(string path)
        {
            addSound(MathTools.LastPortionOfPath(path), Content.Load<SoundEffect>(path));
        }

        public void LoadSound(string path, string name)
        {
            addSound(name, Content.Load<SoundEffect>(path));
        }

        public void LoadSong(string path)
        {
            addSong(MathTools.LastPortionOfPath(path), Content.Load<Song>(path));
        }

        public void LoadSong(string path, string name)
        {
            addSong(name, Content.Load<Song>(path));
        }


        public void addSound(string soundName, SoundEffect sound)
        {
            Sounds.Add(soundName, sound);
        }

        public void addSong(string songName, Song song)
        {
            Songs.Add(songName, song);
        }

        public void playSong(string songName)
        {
            MediaPlayer.Play(getSong(songName));
        }

        public void playSound(string soundName)
        {
            getSound(soundName).Play();
        }


        public Song getSong(string songName)
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

        public SoundEffect getSound(string soundName)
        {
            foreach (var sound in Sounds)
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
