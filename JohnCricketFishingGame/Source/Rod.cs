using Microsoft.Xna.Framework;
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
        private double timer;
        private double maxTimer = 5;
        private RectangleF _bounds;
        //patience acts like the speed of fishing rod and also is the number of turns per round
        // if it reaches 1, the a new round starts with a more difficult costumer
        // 7 Costumers: Kid; Teen; Bachelor holder; Mayor; Priest; Bishop; Colonel, the fearsome Landowner
        private int patience = 14;

        public Vector2 Location;
        public Vector2 Target { get { return Location + Vector2.UnitX * _sprite.Bounds.Width * 0.2f + Vector2.UnitY * 60; } }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_sprite, Location, Color.SaddleBrown);

            spriteBatch.Draw(_targetSprite, Target, Color.MonoGameOrange * 0.85f);
        }
       

        private void MoveRandomly()
        {
            Vector2 newDest = new Vector2(_random.NextSingle(48, 144), _random.NextSingle(80, 100));
            _tweener.TweenTo(target: this, expression: player => player.Location, toValue: newDest, patience, 0.5f)
                .Easing(EasingFunctions.ElasticInOut); 
        }
     
        public void Update(GameTime gt)
        {
            timer += gt.ElapsedGameTime.TotalSeconds;

            _tweener.Update(gt.GetElapsedSeconds());


            _bounds.Position = Target;

            if(timer > maxTimer)
            {
                timer = 0;
                MoveRandomly();
                if(patience > 1)
                {
                    patience--;
                }
            }

            for(int i = 0; i < Game1.fishList.Count; i++)
            {
                if (_bounds.Contains(Game1.fishList[i].Target) && Game1.fishList[i].IsEnabled)
                {
                    Console.WriteLine("Catch!!!!!!!!!");
                    break;
                }
            }

        }

        public Rod()
        {
            Location = new Vector2(96, 32);
            _targetSize = new Size2(7, 7);
            _tweener = new Tweener();
            _random = new FastRandom();
            _targetSprite = Game1.GameContent.Load<Texture2D>("Assets/Art/RodTarget");
            _sprite = Game1.GameContent.Load<Texture2D>("Assets/Art/Rod");
            _bounds = new RectangleF(Target, _targetSize);
        }
    }
}
