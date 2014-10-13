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
    public static partial class EdgeGame
    {
        private static Dictionary<string, SoundEffect> SoundEffects;
        private static Dictionary<string, Song> Songs;

        private static bool IsPlayingPlaylist = false;
        private static int PlaylistIndex = 0;
        private static string PlaylistName = "";
        private static Dictionary<string, List<string>> Playlists;

        public static bool IsRepeating { get { return MediaPlayer.IsRepeating; } set { MediaPlayer.IsRepeating = value; } }
        public static bool IsMuted { get { return MediaPlayer.IsMuted; } set { MediaPlayer.IsMuted = value; } }
        public static float Volume { get { return MediaPlayer.Volume; } set { MediaPlayer.Volume = value; } }
        public static bool IsShuffled { get { return MediaPlayer.IsShuffled; } set { MediaPlayer.IsShuffled = value; } }

        public static void InitializeSounds()
        {
            SoundEffects = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();
            Playlists = new Dictionary<string, List<string>>();

            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        public static void LoadSound(string path)
        {
            AddSound(path.LastSplit('/'), Game.Content.Load<SoundEffect>(path));
        }

        public static void LoadSound(string path, string name)
        {
            AddSound(name, Game.Content.Load<SoundEffect>(path));
        }

        public static void LoadSong(string path)
        {
            AddSong(path.LastSplit('/'), Game.Content.Load<Song>(path));
        }

        public static void LoadSong(string path, string name)
        {
            AddSong(name, Game.Content.Load<Song>(path));
        }


        public static void AddSound(string soundName, SoundEffect sound)
        {
            SoundEffects.Add(soundName, sound);
        }

        public static void AddSong(string songName, Song song)
        {
            Songs.Add(songName, song);
        }

        public static void AddPlaylist(string playlistName, params string[] songNames)
        {
            Playlists.Add(playlistName, songNames.ToList());
        }

        public static void playSong(string songName)
        {
            MediaPlayer.Play(GetSong(songName));
        }

        public static void playPlaylist(string playlistName)
        {
            if (MediaPlayer.IsShuffled)
            {
                PlaylistIndex = RandomTools.RandomInt(0, Playlists[playlistName].Count);
            }

            MediaPlayer.Play(GetSong(Playlists[playlistName][PlaylistIndex]));
            PlaylistName = playlistName;
            IsPlayingPlaylist = true;
        }

        static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                if (PlaylistIndex >= Playlists[PlaylistName].Count - 1 && !MediaPlayer.IsShuffled)
                {
                    PlaylistIndex = 0;

                    if (!MediaPlayer.IsRepeating)
                    {
                        IsPlayingPlaylist = false;
                        return;
                    }
                }

                if (MediaPlayer.IsShuffled)
                {
                    PlaylistIndex = RandomTools.RandomInt(0, Playlists[PlaylistName].Count);
                }
                else
                {
                    PlaylistIndex++;
                }

                MediaPlayer.Play(GetSong(Playlists[PlaylistName][PlaylistIndex++]));
            }
        }

        public static void playSound(string soundName)
        {
            GetSound(soundName).Play();
        }


        public static Song GetSong(string songName)
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

        public static SoundEffect GetSound(string soundName)
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
