using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    public class AudioSystem
    {
        private SoundEffect[] _songs;
        private int _songsCount = 1;
        private static AudioSystem _instance;
        private SoundEffectInstance _sound;
        public static AudioSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioSystem();
                }
                return _instance;
            }
        }

        public enum SongCollection { TitleTheme, MainTheme, GameOver }

        private AudioSystem()
        {
            _songs = new SoundEffect[_songsCount];

            _songs[0] = Game1.GameContent.Load<SoundEffect>("Assets/Audio/1");

            Play(SongCollection.TitleTheme);
        }

        public void Play(SongCollection index)
        {
            _sound = _songs[(int) index].CreateInstance();
            _sound.IsLooped = true;
            _sound.Play();
        }

        public void Pause()
        {
            _sound.Pause();
        }

        public void Update()
        {
            if(_sound.State == SoundState.Paused)
            {
                _sound.Play();
            }
        }
    }
}
