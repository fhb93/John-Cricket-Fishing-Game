using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    // Game statistics and HUD
    public class GameStats
    {
        public enum CustomerLevel { Kid, Teen, Bachelor, Mayor, Priest, Bishop, Colonel }
        public double countDown { get; private set; }
        public bool IsGameOver { get; private set; }
        public static int PlayerScore { get; private set; }
        public static int HighScore { get; private set; }

        private SpriteFont _spriteFont;
        private CustomerLevel _level;
        private double[] countDownTimes;

        private double _suspicion;
        private double[] suspicionPerCustomer;
        private SpriteSheet _suspicionSprite;
        private AnimatedSprite _suspicionAnim;
        private int _bossWarning;
        private int _maxWarnings = 3;
        private Texture2D _bossText;
        private KeyboardState _oldState;


        public GameStats() 
        {
            _spriteFont = Game1.GameContent.Load<SpriteFont>("Assets/Fonts/Font");
            SetChallengeLevel();
            _bossText = Game1.GameContent.Load<Texture2D>("Assets/Art/Notice");
            _suspicionSprite = Game1.GameContent.Load<SpriteSheet>("Assets/Art/Animation/Eye.sf", new JsonContentLoader());
            _suspicionAnim = new AnimatedSprite(_suspicionSprite, "blink");
            if (Game1.ScoreListener == null)
            {
                Game1.ScoreListener += (sender, args) => PlayerScore += 10;
            }
        }

        public void LoadHiScore(int score)
        {
            HighScore = score;
        }

        public void SetChallengeLevel()
        {
            _level = CustomerLevel.Kid;


            if (PlayerScore > 0)
            {
                if(PlayerScore > HighScore)
                {
                    HighScore = PlayerScore;
                    Game1.save.SaveToDevice();
                }
            }

            LoadHiScore(Game1.save.LoadFromDevice());

            PlayerScore = 0;

            _bossWarning = _maxWarnings;

            countDownTimes = new double[7];

            //FINAL BALANCING
            countDownTimes[(int)CustomerLevel.Kid] = 80;
            countDownTimes[(int)CustomerLevel.Teen] = 35;
            countDownTimes[(int)CustomerLevel.Bachelor] = 30;
            countDownTimes[(int)CustomerLevel.Mayor] = 30;
            countDownTimes[(int)CustomerLevel.Priest] = 20;
            countDownTimes[(int)CustomerLevel.Bishop] = 10;
            countDownTimes[(int)CustomerLevel.Colonel] = 5;

            // Balancing for tests
            //countDownTimes[(int)CustomerLevel.Kid] = 9;
            //countDownTimes[(int)CustomerLevel.Teen] = 9;
            //countDownTimes[(int)CustomerLevel.Bachelor] = 8;
            //countDownTimes[(int)CustomerLevel.Mayor] = 7;
            //countDownTimes[(int)CustomerLevel.Priest] = 6;
            //countDownTimes[(int)CustomerLevel.Bishop] = 5;
            //countDownTimes[(int)CustomerLevel.Colonel] = 4;

            countDown = countDownTimes[(int)_level];

            suspicionPerCustomer = new double[7];

            //Alternate balancing
            //suspicionPerCustomer[(int)CustomerLevel.Kid] = 9;
            //suspicionPerCustomer[(int)CustomerLevel.Teen] = 9;
            //suspicionPerCustomer[(int)CustomerLevel.Bachelor] = 8;
            //suspicionPerCustomer[(int)CustomerLevel.Mayor] = 7;
            //suspicionPerCustomer[(int)CustomerLevel.Priest] = 6;
            //suspicionPerCustomer[(int)CustomerLevel.Bishop] = 5;
            //suspicionPerCustomer[(int)CustomerLevel.Colonel] = 4;


            suspicionPerCustomer[(int)CustomerLevel.Kid] = 8;
            suspicionPerCustomer[(int)CustomerLevel.Teen] = 7;
            suspicionPerCustomer[(int)CustomerLevel.Bachelor] = 6;
            suspicionPerCustomer[(int)CustomerLevel.Mayor] = 5;
            suspicionPerCustomer[(int)CustomerLevel.Priest] = 5;
            suspicionPerCustomer[(int)CustomerLevel.Bishop] = 4;
            suspicionPerCustomer[(int)CustomerLevel.Colonel] = 3;

            _suspicion = 0;

        }

        /// <summary>
        /// Rendering HUD here
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            string stopwatchStr = string.Format("{0}:{1}s", _level.ToString(), (int) countDown);
            string scoreStr = string.Format("$ {0}", PlayerScore);
            Vector2 stopwatchPos = new Vector2(192 / 16f, 8);
            Vector2 scorePos = new Vector2(192 * 9 / 16f, 8);
           
            //HUD
            sb.DrawString(_spriteFont, stopwatchStr, stopwatchPos, Color.White);
            sb.DrawString(_spriteFont, scoreStr, scorePos, Color.White);
            DrawNotice(sb);
            DrawSuspicion(sb);
        }

        private void DrawSuspicion(SpriteBatch sb)
        {
            Vector2 suspicionPos = new Vector2(192 / 16f, 16);

            Vector2 suspicionEyePos = new Vector2(192 / 16f + 4, 16 + 4);

            for (int i = 0; i < (int)suspicionPerCustomer[(int)_level]; i++)
            {
                Vector2 Location = suspicionPos + Vector2.UnitX * 16 + Vector2.UnitX * i * 6;
                Size2 size = new Size2(6, 8);
                sb.FillRectangle(new RectangleF(Location, size), (i + 1) <= _suspicion ? Color.Red * 0.5f : Color.Gold);
            }

            sb.Draw(_suspicionAnim, suspicionEyePos);
        }

        private void DrawNotice(SpriteBatch sb)
        {
            Vector2 warningsPos = new Vector2(192 * 13 / 16f, 8);

            for (int i = 0; i < _maxWarnings; i++)
            {
                sb.Draw(_bossText, warningsPos + Vector2.UnitX * 10 * i, (i + 1) <= _bossWarning ? Color.Gold : Color.Red * 0.5f);
            }
        }

        public void Update(GameTime gt)
        {
            var deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;

            if(_suspicion > (int)(suspicionPerCustomer[(int)_level] * 0.75f))
            {
                _suspicionAnim.Color = Color.Red;
                _suspicionAnim.TextureRegion = new TextureRegion2D(_suspicionSprite.TextureAtlas.Texture, new Rectangle(0, 0, 8, 8));
            }
            else
            {
                _suspicionAnim.Color = Color.White;
                _suspicionAnim.Update(deltaTime);
            }

            if (_bossWarning == 0)
            {
                IsGameOver = true;
            }

            if (countDown > 0.01f)
            {
                countDown -= gt.ElapsedGameTime.TotalSeconds;

                if(Fish.fishList.Count == 0)
                {
                    LevelUpCustomer();
                }
            }
            else
            {
                // while there are still some levels available to play
                if(_level < CustomerLevel.Colonel)
                {
                    LevelUpCustomer();
                }
                // otherwise, finish the game and checks if there is a new high score
                else
                {
                    GameOverSetup();
                }
            }
        }

        private void LevelUpCustomer()
        {
            AudioSystem.Instance.Play(AudioSystem.SFXCollection.Start);

            Fish.AddFishs(8);
            _level++;
            countDown = countDownTimes[(int)_level];
            _suspicion = 0;
        }

        private void GameOverSetup()
        {
            IsGameOver = true;

            if (HighScore < PlayerScore)
            {
                HighScore = PlayerScore;
                MenuGameOver.NewRecord = true;
            }
        }

        public void IncreaseCustomerSuspicion(GameTime gt)
        {
            if(_suspicion < suspicionPerCustomer[(int) _level])
            {
                _suspicion += gt.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _suspicion = 0;
                _bossWarning--;
            }
        }

        public void ResetGame()
        {
            if (HighScore > Game1.save.LoadFromDevice())
            {
                Game1.save.SaveToDevice();
            }

            SetChallengeLevel();

            IsGameOver = false;
        }

        public void LowerSuspicion(GameTime gt)
        {
            _suspicion = _suspicion > 0.1f ? _suspicion - gt.ElapsedGameTime.TotalSeconds * 2f : 0;
        }
    }
}
