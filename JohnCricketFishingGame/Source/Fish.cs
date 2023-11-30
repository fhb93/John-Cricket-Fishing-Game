using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JohnCricketFishingGame.Source
{
    internal class Fish
    {
        private Texture2D _texture;

        private RectangleF _boundingBox;

        public Vector2 _cornerPosition { get; private set; }

        private float _scale;

        private Tweener _tweener;

        public Vector2 Location => _cornerPosition - Vector2.UnitX * _boundingBox.Width * 0.5f -
            Vector2.UnitY * _boundingBox.Height * 0.5f;


        public Fish()
        {
            _scale = 1;

            _texture = Game1.GameContent.Load<Texture2D>("Assets/Art/Fish");

            _cornerPosition = new Vector2(192 / 2, 160 / 2);

            _boundingBox = new RectangleF(Location.X, Location.Y, _texture.Bounds.Width * _scale, _texture.Bounds.Height * _scale);

            _tweener = new Tweener();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime = null)
        {
            spriteBatch.Draw(_texture, _boundingBox.ToRectangle(), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            _boundingBox.Position = Location;
        }

        public void Move(Vector2 destination)
        {
            _cornerPosition += destination;
        }
    }
}
