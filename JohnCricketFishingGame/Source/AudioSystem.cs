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

        private AudioSystem()
        {
            _songs = new SoundEffect[_songsCount];

            _songs[0] = Game1.GameContent.Load<SoundEffect>("Assets/Audio/1");

            Play(0);
        }

        public void Play(int index)
        {
            SoundEffectInstance se = _songs[index].CreateInstance();
            se.IsLooped = true;
            se.Play();
        }

        public void Update()
        {
            
        }
    }
}
