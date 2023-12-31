﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tweening;
using System;


namespace JohnCricketFishingGame.Source
{
    public class Rod
    {
        private Texture2D _sprite;
        private Texture2D _targetSprite;
        private FastRandom _random;
        private Tweener _tweener;
        private Size2 _targetSize;
        private double _timer;
        private double _maxTimer = 5;
        private const int _startingPatience = 8;
        private RectangleF _bounds;
        private RectangleF _validArea;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        //patience acts like the speed of fishing rod and also is the number of turns per round
        // if it reaches 1, the a new round starts with a more difficult customer
        // 7 customers: Kid; Teen; Bachelor holder; Mayor; Priest; Bishop; Colonel, the fearsome Landowner
        private int _patience = _startingPatience;

        public Vector2 Location;
        public Vector2 Target { get { return Location - Vector2.UnitX * _sprite.Bounds.Width * 0.2f + Vector2.UnitY * 60; } }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, Location, Color.LightGray);
            spriteBatch.Draw(_targetSprite, Target, Color.Red * 0.85f);

            //for Debug 
            //spriteBatch.FillRectangle(_validArea, Color.Red * 0.5f);
        }

        private void MoveRandomly()
        {
            Vector2 newDest = new Vector2(_random.NextSingle(_minX, _maxX), _random.NextSingle(_minY, _maxY));
               
            _tweener.TweenTo(target: this, expression: player => player.Location, toValue: newDest, _patience, 0.5f)
                    .Easing(EasingFunctions.ElasticInOut);
        }


        public void Update(GameTime gt)
        {
            _timer += gt.ElapsedGameTime.TotalSeconds;

            _tweener.Update(gt.GetElapsedSeconds());

            _bounds.Position = Target;

            if (_timer > _maxTimer)
            {
                _timer = 0;
                MoveRandomly();

                if(_patience > 1)
                {
                    _patience--;
                }
            }

            for(int i = 0; i < Fish.fishList.Count; i++)
            {
                if (_bounds.Contains(Fish.fishList[i].Target) && Fish.fishList[i].IsEnabled)
                {
                    // i is the fish we want to remove
                    Game1.ScoreListener.Invoke(this, i);
                    Fish.fishList[i].RemoveFish(i);
                    break;
                }
            }
        }

        public Rod()
        {
            _tweener = new Tweener();
            int seed = Math.Abs(Guid.NewGuid().GetHashCode());
            _random = new FastRandom(seed);
            _targetSprite = Game1.GameContent.Load<Texture2D>("Assets/Art/RodTarget");
            _sprite = Game1.GameContent.Load<Texture2D>("Assets/Art/Rod");
            _validArea = new Rectangle(48, 20, 112, 50);
            _minX = _validArea.X;
            _maxX = _validArea.X + _validArea.Width;
            _minY = _validArea.Y;
            _maxY = _validArea.Y + _validArea.Height;
            ResetRod();
            Game1.ScoreListener += (sender, args) => ResetRod();
        }

        public void ResetRod()
        {
            Location = new Vector2(96, 32);
            _targetSize = new Size2(7, 7);
            _bounds = new RectangleF(Target, _targetSize);
            _patience = _startingPatience;

        }
    }
}
