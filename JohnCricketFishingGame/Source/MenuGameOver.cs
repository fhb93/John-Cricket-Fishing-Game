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
    public class MenuGameOver : Menu
    {
        private Label _screenTitle;
        private Label _newRecordLabel;
        private Label _niceTryLabel;
        private Label _screenPrompt;
        public static bool NewRecord;

        public MenuGameOver()
        {
            Vector2 titlePos = new Vector2(192 / 2, 160 * 5 / 16);
            _screenTitle = new Label("Game Over", titlePos);
            _screenPrompt = new Label("Press Space", titlePos + Vector2.UnitY * 160 * 4 / 16);
            _newRecordLabel = new Label("New Record!", titlePos + Vector2.UnitY * 160 * 2 / 16);
            _niceTryLabel = new Label("Nice try!", titlePos + Vector2.UnitY * 160 * 2 / 16);
            maxTimer = 2;
            NewRecord = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(_screenTitle.SpriteFont, _screenTitle.Title, _screenTitle.Pos, NewRecord ? Color.Gold : Color.White);

            if (NewRecord)
            {
                sb.DrawString(_newRecordLabel.SpriteFont, _newRecordLabel.Title, _newRecordLabel.Pos, Color.Gold);
            }
            else
            {
                sb.DrawString(_niceTryLabel.SpriteFont, _niceTryLabel.Title, _niceTryLabel.Pos, Color.White);
            }

            sb.DrawString(_screenPrompt.SpriteFont, _screenPrompt.Title, _screenPrompt.Pos, timer <= 1f ? Color.White : Color.Transparent);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }
    }
}
