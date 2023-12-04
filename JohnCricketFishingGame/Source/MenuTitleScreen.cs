using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Timers;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    public class MenuTitleScreen : Menu
    {
        private SpriteSheet _sprite;
        private AnimatedSprite _animatedSprite;
        private Label _title;
        
        private Label _credits;
        private Color[] _colors = { Color.Black, Color.SaddleBrown, Color.Goldenrod, Color.Gold };
        private Label _prompt;
        private Label _hiScore;
        private float _creditsEndTimer;
        private int _colorsIndex;
        private float _timeFade;

        public static bool DoOnceIsSet = false;

        public MenuTitleScreen()
        {
            _sprite = Game1.GameContent.Load<SpriteSheet>("Assets/Art/Animation/JohnCricketCover.sf", new JsonContentLoader());
            _animatedSprite = new AnimatedSprite(_sprite);
            _animatedSprite.Color = Color.LightGray;
            _animatedSprite.Play("menu");

            _credits = new Label("   A Game by   \n\nFelipe  Bezerra\n\n(MonoGameJam 5)\n\n\n    - 2023 -    ", new Vector2(192 / 2, 160f * 8f / 16));
           
            //John Cricket
            //João Grilo
            _title = new Label("John Cricket's\nAmazing Game!!", new Vector2(192 / 2, 160f * 2f / 16));
            _hiScore = new Label($"HI-SCORE: {GameStats.HighScore}", new Vector2(192 / 2, 160 * 13 / 16));
            _prompt = new Label("Press Space", new Vector2(192 / 2, 160f * 14 / 16));
            maxTimer = 2;
            _creditsEndTimer = 0f;
            _timeFade = 0;
        }


        public override void Draw(SpriteBatch sb)
        {
            if (DoOnceIsSet == false)
            {
                if (_timeFade < 6f)
                {
                    sb.DrawString(_credits.SpriteFont, _credits.Title, _credits.Pos, _colors[_colorsIndex]);
                    return;
                }
            }

            DoOnceIsSet = true;


            sb.Draw(_animatedSprite, new Vector2(192 / 2, 160 / 2), 0f, Vector2.One * 2f);

            sb.DrawString(_title.SpriteFont, _title.Title, _title.Pos, Color.White);

            sb.DrawString(_hiScore.SpriteFont, _hiScore.Title, _hiScore.Pos, Color.White);

            sb.DrawString(_prompt.SpriteFont, _prompt.Title, _prompt.Pos, timer <= 1f ? Color.Transparent : Color.White);

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            var deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;

            if (DoOnceIsSet == false)
            {
                _timeFade += deltaTime;

                if (_timeFade < 3f)
                {
                    if (_creditsEndTimer < 3)
                    {
                        _creditsEndTimer += deltaTime * 2f;
                    }
                }
                else
                {
                    if (_creditsEndTimer > 0)
                    {
                        _creditsEndTimer -= deltaTime * 2f;
                    }
                }
            }

            _colorsIndex = (int) _creditsEndTimer;
            _animatedSprite.Update(deltaTime);
        }
    }
}
