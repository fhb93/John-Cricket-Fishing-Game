using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    public class MenuGameOver : Menu
    {
        public Label _screenTitle;
        public Label _screenPrompt;

        public MenuGameOver() : base()
        {
            Vector2 titlePos = new Vector2(192 / 2, 160 * 5 / 16);
            _screenTitle = new Label("Game Over", titlePos);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(_screenTitle.SpriteFont, _screenTitle.Title, _screenTitle.Pos, Color.White);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
    }
}
