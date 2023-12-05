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
        private SoundEffect[] _sfxs;
        private int _songsCount = 1;
        private int _sfxsCount = 3;
        private static AudioSystem _instance;
        private SoundEffectInstance _sound;
        private SoundEffectInstance _sfxSound;
        private float _mixerSFXVol;
        private float _mixerOSTVol;
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
        public enum SFXCollection { Start, Gold, Reset }

        private AudioSystem()
        {
            _songs = new SoundEffect[_songsCount];
            _sfxs = new SoundEffect[_sfxsCount];

            _songs[0] = Game1.GameContent.Load<SoundEffect>("Assets/Audio/1");
            _sfxs[0] = Game1.GameContent.Load<SoundEffect>("Assets/Audio/Start");
            _sfxs[1] = Game1.GameContent.Load<SoundEffect>("Assets/Audio/Gold");
            _sfxs[2] = Game1.GameContent.Load<SoundEffect>("Assets/Audio/Reset");
            _mixerSFXVol = 0.01f;
            _mixerOSTVol = 0.07f;

            Play(SongCollection.TitleTheme);
        }

        public void Play(SongCollection index)
        {
            _sound = _songs[(int) index].CreateInstance();
            _sound.IsLooped = true;
            _sound.Volume = _mixerOSTVol;
            _sound.Play();
        }

        public void Play(SFXCollection index)
        {
            _sfxSound = _sfxs[(int)index].CreateInstance();
            _sfxSound.IsLooped = false;
            _sfxSound.Volume = _mixerSFXVol;
            _sfxSound.Play();
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
