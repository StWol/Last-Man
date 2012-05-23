using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace EVCS_Projekt.Audio
{
    class MusicPlayer
    {

        public static Song LookBehind { get; private set; }

        public static void LoadSongs()
        {
            LookBehind = Main.ContentManager.Load<Song>("music/Greendjohn_-_Look_Behind");
        }

        public static void Play(Song song)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}
