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

        public MenuGameOver()
        {
            Vector2 titlePos = new Vector2(192 / 2, 160 * 5 / 16);
            _screenTitle = new Label("Game Over", titlePos);
            _screenPrompt = new Label("Press Space", titlePos + Vector2.UnitY * 160 * 2 / 16);
            maxTimer = 2;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(_screenTitle.SpriteFont, _screenTitle.Title, _screenTitle.Pos, Color.White);
            sb.DrawString(_screenPrompt.SpriteFont, _screenPrompt.Title, _screenPrompt.Pos, timer <= 1f ? Color.White : Color.Transparent);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
    }
}
