using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    class Menu
    {
        private SpriteSheet _sprite;
        private AnimatedSprite _animatedSprite;
        
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

        private Label _title;
        private Label _prompt;

        private float _maxTimer;
        private float _timer;

        public Menu()
        {
            _sprite = Game1.GameContent.Load<SpriteSheet>("Assets/Art/Animation/JohnCricketCover.sf", new JsonContentLoader());
            _animatedSprite = new AnimatedSprite(_sprite);
            _animatedSprite.Color = Color.LightGray;
            _animatedSprite.Play("menu");

            _title = new Label("John Cricket's\nAmazing Game!!", new Vector2(192 / 2, 160f * 2 / 16));
            _prompt = new Label("Press Space", new Vector2(192 / 2, 160f * 13 / 16));
            _maxTimer = 2;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_animatedSprite, new Vector2(192 / 2, 160 / 2), 0f, Vector2.One * 2f);

            sb.DrawString(_title.SpriteFont, _title.Title, _title.Pos, Color.White);

            sb.DrawString(_prompt.SpriteFont, _prompt.Title, _prompt.Pos, _timer <= 1f ? Color.Transparent : Color.White);
        }

        public void Update(GameTime gt)
        {
            var deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;

            _timer += deltaTime;

            if(_timer > _maxTimer)
            {
                _timer = 0f;
            }


            _animatedSprite.Update(deltaTime);
        }
    }
}
