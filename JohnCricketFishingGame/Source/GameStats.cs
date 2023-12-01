using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public enum CostumerLevel { Kid, Teen, Undergraduate, Mayor, Priest, Bishop, Colonel }
        public double countDown { get; private set; }
        public bool IsGameOver { get; private set; }

        private SpriteFont _spriteFont;
        private CostumerLevel _level;
        private double[] countDownTimes;

        public GameStats() 
        {
            _spriteFont = Game1.GameContent.Load<SpriteFont>("Assets/Fonts/Font");
            SetChallengeLevel();
        }

        public void SetChallengeLevel()
        {
            _level = CostumerLevel.Kid;

            countDownTimes = new double[7];
            countDownTimes[(int)CostumerLevel.Kid] = 45;
            countDownTimes[(int)CostumerLevel.Teen] = 40;
            countDownTimes[(int)CostumerLevel.Undergraduate] = 35;
            countDownTimes[(int)CostumerLevel.Mayor] = 30;
            countDownTimes[(int)CostumerLevel.Priest] = 25;
            countDownTimes[(int)CostumerLevel.Bishop] = 20;
            countDownTimes[(int)CostumerLevel.Colonel] = 15;

            countDown = countDownTimes[(int)_level];
        }

        // HUD
        public void Draw(SpriteBatch sb)
        {
            string stopwatchStr = string.Format("{0}", (int) countDown);
            Vector2 position = new Vector2(192 / 2f - _spriteFont.MeasureString(stopwatchStr).X * 0.5f, 16);
            sb.DrawString(_spriteFont, stopwatchStr, position, Color.White);
        }

        public void Update(GameTime gt)
        {
            if (countDown > 0.1f)
            {
                countDown -= gt.ElapsedGameTime.TotalSeconds * 4;
            }
            else
            {
                IsGameOver = true;
            }
        }
    }
}
