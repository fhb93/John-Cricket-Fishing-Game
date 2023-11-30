using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Timers;
using MonoGame.Extended.Tweening;
using MonoGame.Extended.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats.Asn1;

namespace JohnCricketFishingGame.Source
{
    public class Fish
    {
        private SpriteSheet _sprite;

        private Texture2D _playField;

        private AnimatedSprite _animatedSprite;

        private RectangleF _boundingBox;

        private float _scale;

        private Tweener _tweener;


        public bool IsEnabled { get; private set; }
        public Vector2 _cornerPosition { get; private set; }
        public Vector2 Target { get { return _boundingBox.Center - Vector2.UnitY * 16; } }

        //public Vector2 Location => _cornerPosition - Vector2.UnitX * _boundingBox.Width * 0.5f -
        //  Vector2.UnitY * _boundingBox.Height * 0.5f;
        public Vector2 Location { get; private set; }
          

        public void Draw(SpriteBatch spriteBatch, bool isPlayerControlled = false, GameTime gameTime = null)
        {
            spriteBatch.Draw(_playField, new Rectangle(0,40, 192, 120), Color.White);

            if(isPlayerControlled)
            {
                _animatedSprite.Color = Color.Yellow;
            }

            spriteBatch.Draw(_animatedSprite, Location);
            spriteBatch.DrawCircle(Target, 8, 8, Color.Red * 0.6f);
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            _boundingBox.Position = Location;

            _animatedSprite.Update(deltaTime);
        }

        public void Move(Vector2 destination)
        {
            Location += destination * 15;
        }

        public void UpdateFishActivation(bool isOn)
        {
            IsEnabled = isOn;
        }

        public Fish()
        {
            _scale = 1;

            _sprite = Game1.GameContent.Load<SpriteSheet>("Assets/Art/Animation/Fish.sf", new JsonContentLoader());
            _playField = Game1.GameContent.Load<Texture2D>("Assets/Art/Playfield");
            _animatedSprite = new AnimatedSprite(_sprite);

            _cornerPosition = new Vector2(192 / 2, 160 * 3 / 4);

            _boundingBox = _animatedSprite.GetBoundingRectangle(_animatedSprite.Origin, 0f, Vector2.One);

            Location = _cornerPosition - Vector2.UnitX * _boundingBox.Width * 0.5f -
              Vector2.UnitY * _boundingBox.Height * 0.5f;

            _tweener = new Tweener();

            _animatedSprite.Play("walk");
        }
    }
}
