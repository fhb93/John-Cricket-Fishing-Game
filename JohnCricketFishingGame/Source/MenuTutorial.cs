using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JohnCricketFishingGame.Source
{
    public class MenuTutorial : Menu
    {
        private Label _concept;
        private Label _controls;
        private string _controlsContents = "- CONTROLS - \n\nCHOOSE A FISH:\n" +
            "WASD or Arrow Keys\n\n" +
            "MOVE A FISH:\n" +
            "Hold SPACE while\nWASD or Arrow Keys\n\n" +
            "PAUSE / UNPAUSE: P\n\n" +
            "SKIP INTRO: SPACE";

        private string _conceptContents =
            "It's the Feast of\n\n" +
            "St. John the Baptist!\n\n" +
            "You're John Cricket a\n\n" +
            "regular guy working\n\n" +
            "in the fishing\n\n" +
            "game booth!\n\n" +
            "Move the fish dolls\n\n" +
            "to help the customers\n\n" +
            "catch them. Careful as\n\n" +
            "they can be\n\n" +
            "aware of your rigging.\n\n" +
            "Each customer is\n\n" +
            "more challenging.\n\n" +
            "So be skillful\n\n" +
            "when rigging their\n\n" +
            "games...\n";

        public MenuTutorial()
        {
            _concept = new Label(_conceptContents, new Vector2(192 / 2, 160 * 2));
            _controls = new Label(_controlsContents, new Vector2(192 / 2, 160 * 1.2f));
            maxTimer = 2.25f;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            _controls.MoveTo(_controls.Pos - Vector2.UnitY * 30 * (float)gt.ElapsedGameTime.TotalSeconds);
                
            if (_controls.Pos.Y < (160 * 1 / 4))
            {
                _concept.MoveTo(_concept.Pos - Vector2.UnitY * 25 * (float)gt.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(_concept.SpriteFont, _concept.Title, _concept.Pos, Color.White);
            sb.DrawString(_controls.SpriteFont, _controls.Title, _controls.Pos, timer <= 1 ? Color.LightGray : Color.Yellow);
        }
    }
}
