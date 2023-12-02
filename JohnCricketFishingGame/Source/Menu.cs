using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JohnCricketFishingGame.Source
{
    public class Menu
    {
        public struct Label
        {
            public string Title { get; private set; }
            public Vector2 Pos { get; private set; }
            public SpriteFont SpriteFont { get; private set; }

            public Label(string str, Vector2 pos)
            {
                SpriteFont = Game1.GameContent.Load<SpriteFont>("Assets/Fonts/Font");
                Title = str;
                Pos = pos - Vector2.UnitX * SpriteFont.MeasureString(Title).X * 0.5f;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {

        }

        public virtual void Update(GameTime gt)
        {

        }
    }
}
