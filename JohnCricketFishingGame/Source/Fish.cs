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
        private RectangleF _validArea;
        private Color playfieldColor;

        private float _scale;
        private float _maxTimer;
        private float _timer;
        private bool _isCaught;

        private Tweener _tweener;

        public bool IsEnabled { get; private set; }
        public Vector2 _cornerPosition { get; private set; }
        public Vector2 Target { get { return _boundingBox.Center - Vector2.UnitY * 16; } }

        //public Vector2 Location => _cornerPosition - Vector2.UnitX * _boundingBox.Width * 0.5f -
        //  Vector2.UnitY * _boundingBox.Height * 0.5f;
        public Vector2 Location { get; private set; }
        public static List<Fish> fishList = new List<Fish>();


        public void Draw(SpriteBatch spriteBatch, GameTime gameTime = null)
        {
            spriteBatch.Draw(_playField, new Rectangle(0, 40, 192, 120), playfieldColor);

            _animatedSprite.Color = Color.White;

            Color targetColor = Color.LightSalmon;
            
            if (IsEnabled == true)
            {
                _animatedSprite.Color = Color.Yellow;
                targetColor = Color.Yellow;
            }
            if(_isCaught)
            {
                _animatedSprite.Color = Color.Transparent;
                targetColor = Color.Transparent;
            }

            spriteBatch.Draw(_animatedSprite, Location);

            spriteBatch.DrawCircle(Target, 8, 4, targetColor * 0.2f);
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            _timer += deltaTime;

            _boundingBox.Position = Location;

            _animatedSprite.Update(deltaTime);
        }

        public void Move(Vector2 destination)
        {
            if (_validArea.Contains(Location + destination * 15))
            {
                playfieldColor = Color.Gray;

                Location += destination * 15;
            }
            else
            {
                playfieldColor = Color.Red;
            }
        }

        public void UpdateFishActivation(bool isOn)
        {
            if (_isCaught == false)
            {
                IsEnabled = isOn;
            }
        }

        public void RemoveFish(int index)
        {

            fishList[index]._isCaught = true;

            for (int i = 0; i < fishList.Count; i++)
            {
                fishList[i].UpdateFishActivation(false);
            }

            fishList.RemoveAt(index);
        }

        public static void AddFishs(int fishCount)
        {
            fishList.Clear();

            for (int i = 0; i < fishCount; i++)
            {
                fishList.Add(new Fish(i));
            }

            UpdateFishStatus(0);
        }

        public static void UpdateFishStatus(int focusedFish = 0)
        {
            for (int i = 0; i < Fish.fishList.Count; i++)
            {
                fishList[i].UpdateFishActivation(false);

                if(focusedFish == i)
                {
                    fishList[i].UpdateFishActivation(true);
                }

            }
        }

        public Fish(int offset)
        {
            _scale = 1;
            _maxTimer = 3;
            _sprite = Game1.GameContent.Load<SpriteSheet>("Assets/Art/Animation/Fish.sf", new JsonContentLoader());
            _playField = Game1.GameContent.Load<Texture2D>("Assets/Art/Playfield");
            _animatedSprite = new AnimatedSprite(_sprite);

            Vector2 startPos = Vector2.Zero;

            // offset is less than half fish plus one
            if(offset < 4)
            {
                startPos += Vector2.UnitX * ((192 / 4) + offset * 32);
                startPos += Vector2.UnitY * (160 * 2 / 4);
            }
            else
            {
                int tempOffset = offset - 4;
                startPos += Vector2.UnitX * ((192 / 4) + tempOffset * 32);
                startPos += Vector2.UnitY * (160 * 3 / 4);
            }

            _cornerPosition = startPos;

            _boundingBox = _animatedSprite.GetBoundingRectangle(_animatedSprite.Origin, 0f, Vector2.One);
           
            _validArea = new Rectangle(22, 44, 145, 100);

            Location = _cornerPosition - Vector2.UnitX * _boundingBox.Width * 0.5f -
              Vector2.UnitY * _boundingBox.Height * 0.5f;

            _tweener = new Tweener();

            IsEnabled = false;

            _animatedSprite.Play("walk");
        }
    }
}
