using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
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
        public enum CustomerLevel { Kid, Teen, Undergraduate, Mayor, Priest, Bishop, Colonel }
        public double countDown { get; private set; }
        public bool IsGameOver { get; private set; }
        public int PlayerScore { get; private set; }

        private SpriteFont _spriteFont;
        private CustomerLevel _level;
        private double[] countDownTimes;

        private double _suspicion;
        private double[] suspicionPerCustomer;
        private int _bossWarning = 3;

        public GameStats() 
        {
            _spriteFont = Game1.GameContent.Load<SpriteFont>("Assets/Fonts/Font");
            PlayerScore = 0;
            SetChallengeLevel();
            Game1.ScoreListener += (sender, args) => PlayerScore++;
        }

        public void SetChallengeLevel()
        {
            _level = CustomerLevel.Kid;

            _bossWarning = 3;

            countDownTimes = new double[7];

            //FINAL BALANCING
            //countDownTimes[(int)CustomerLevel.Kid] = 45;
            //countDownTimes[(int)CustomerLevel.Teen] = 40;
            //countDownTimes[(int)CustomerLevel.Undergraduate] = 35;
            //countDownTimes[(int)CustomerLevel.Mayor] = 30;
            //countDownTimes[(int)CustomerLevel.Priest] = 25;
            //countDownTimes[(int)CustomerLevel.Bishop] = 20;
            //countDownTimes[(int)CustomerLevel.Colonel] = 15;

            countDownTimes[(int)CustomerLevel.Kid] = 10;
            countDownTimes[(int)CustomerLevel.Teen] = 9;
            countDownTimes[(int)CustomerLevel.Undergraduate] = 8;
            countDownTimes[(int)CustomerLevel.Mayor] = 7;
            countDownTimes[(int)CustomerLevel.Priest] = 6;
            countDownTimes[(int)CustomerLevel.Bishop] = 5;
            countDownTimes[(int)CustomerLevel.Colonel] = 4;

            countDown = countDownTimes[(int)_level];

            suspicionPerCustomer = new double[7];

            //FINAL BALANCING
            //suspicionPerCustomer[(int)CustomerLevel.Kid] = 9;
            //suspicionPerCustomer[(int)CustomerLevel.Teen] = 9;
            //suspicionPerCustomer[(int)CustomerLevel.Undergraduate] = 8;
            //suspicionPerCustomer[(int)CustomerLevel.Mayor] = 7;
            //suspicionPerCustomer[(int)CustomerLevel.Priest] = 6;
            //suspicionPerCustomer[(int)CustomerLevel.Bishop] = 5;
            //suspicionPerCustomer[(int)CustomerLevel.Colonel] = 4;


            suspicionPerCustomer[(int)CustomerLevel.Kid] = 8;
            suspicionPerCustomer[(int)CustomerLevel.Teen] = 7;
            suspicionPerCustomer[(int)CustomerLevel.Undergraduate] = 6;
            suspicionPerCustomer[(int)CustomerLevel.Mayor] = 5;
            suspicionPerCustomer[(int)CustomerLevel.Priest] = 4;
            suspicionPerCustomer[(int)CustomerLevel.Bishop] = 3;
            suspicionPerCustomer[(int)CustomerLevel.Colonel] = 2;

            _suspicion = 0;

        }

        // HUD
        public void Draw(SpriteBatch sb)
        {
            string stopwatchStr = string.Format("{0}:{1}s", _level.ToString(), (int) countDown);
            string suspicionStr = string.Format("{0}/{1}", (int) _suspicion, (int)suspicionPerCustomer[(int)_level]);
            string scoreStr = string.Format("$ {0}", (int) PlayerScore * 100);
            Vector2 stopwatchPos = new Vector2(192 / 16f, 8);
            Vector2 suspicionPos = new Vector2(192 / 16f, 16);
            Vector2 scorePos = new Vector2(192 * 9 / 16f - _spriteFont.MeasureString(scoreStr).X * 0.5f, 8);
            
            Vector2 warningsPos = new Vector2(192 * 13 / 16f - _spriteFont.MeasureString("# # #").X * 0.5f, 8);

            sb.DrawString(_spriteFont, stopwatchStr, stopwatchPos, Color.White);
            sb.DrawString(_spriteFont, suspicionStr, suspicionPos, Color.White);
            sb.DrawString(_spriteFont, scoreStr, scorePos, Color.White);
            
            for(int i = 0; i < _bossWarning; i++)
            {
                sb.DrawString(_spriteFont, "# ", warningsPos + Vector2.UnitX * 8 * i, Color.White);
            }
        }

        public void Update(GameTime gt)
        {
            if(_bossWarning == 0)
            {
                IsGameOver = true;
            }

            if (countDown > 0.1f)
            {
                countDown -= gt.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _level++;
                countDown = countDownTimes[(int)_level];
                _suspicion = 0;
               // IsGameOver = true;
            }
        }

        public void CustomerSuspicion(GameTime gt)
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

        public void LowerSuspicion(GameTime gt)
        {
            _suspicion = _suspicion > 0.1f ? _suspicion - gt.ElapsedGameTime.TotalSeconds * 2f : 0;
        }
    }
}
