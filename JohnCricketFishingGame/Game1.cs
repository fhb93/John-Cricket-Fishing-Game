using JohnCricketFishingGame.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Tweening;
using System;
using System.Collections.Generic;
using System.Text;

namespace JohnCricketFishingGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget2D;

        public static ContentManager GameContent;
        public static GraphicsDeviceManager GameGraphics;
        
        //Game stuff
        private List<Fish> fishList;
        private int _fishCount = 1;
        private Effect _effect;
        private Tweener _tweener;
        private readonly GamePadListener _gamePadListener;
        private readonly KeyboardListener _keyboardListener;

        private int vcsWidth = 192;
        private int vcsHeight = 160;
        private int ScreenWidth = 1024;
        private int ScreenHeight = 768;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            IsMouseVisible = true;


            GameContent = Content;
            GameGraphics = _graphics;
            _gamePadListener = new GamePadListener();
            _keyboardListener = new KeyboardListener();
            Components.Add(new InputListenerComponent(this, _keyboardListener, _gamePadListener));

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            fishList = new List<Fish>();

            _tweener = new Tweener();
            _renderTarget2D = new RenderTarget2D(
                GraphicsDevice,
                vcsWidth, vcsHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < _fishCount; i++)
            {
                fishList.Add(new Fish()); 
            }

            _effect = Content.Load<Effect>("Assets/Shaders/crt-lottes-mg");

            SetupEffect();
        }

           
        Vector2 destination = Vector2.Zero;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            _keyboardListener.KeyPressed += (sender, args) => destination = HandleInput(args.Key); //{ Window.Title = $"Key {args.Key} Pressed"; };
            _gamePadListener.ButtonDown += (sender, args) => { Window.Title = $"Key {args.Button} Down"; };

            _tweener.Update(gameTime.GetElapsedSeconds());


            for (int i = 0; i < fishList.Count;i++)
            {
                fishList[i].Update(gameTime);
                fishList[i].Move(destination * gameTime.GetElapsedSeconds());
            }


            base.Update(gameTime);
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            for (int i = 0; i < fishList.Count; i++)
            {
                fishList[i].Draw(_spriteBatch);
            }

            _spriteBatch.End();

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Draw(GameTime gameTime)
        {
            DrawSceneToTexture(_renderTarget2D);

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointClamp, DepthStencilState.Default,
                RasterizerState.CullNone, _effect);

            _spriteBatch.Draw(_renderTarget2D, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void SetupEffect()
        {

            _effect.Parameters["hardScan"]?.SetValue(-16f);
            _effect.Parameters["hardPix"]?.SetValue(-8.0f);
            _effect.Parameters["warpX"]?.SetValue(0.31f); // 0.031
            _effect.Parameters["warpY"]?.SetValue(0.41f);
            _effect.Parameters["maskDark"]?.SetValue(0.5f); //0
            _effect.Parameters["maskLight"]?.SetValue(1.5f); //1.5
            _effect.Parameters["scaleInLinearGamma"]?.SetValue(1f);
            _effect.Parameters["shadowMask"]?.SetValue(1.0f);
            _effect.Parameters["brightboost"]?.SetValue(0.18f); //1.8
            _effect.Parameters["hardBloomScan"]?.SetValue(-1f);
            _effect.Parameters["hardBloomPix"]?.SetValue(-2.0f);
            _effect.Parameters["bloomAmount"]?.SetValue(4f);//0.15f);
            _effect.Parameters["shape"]?.SetValue(10.0f);
            _effect.Parameters["warp"]?.SetValue(new Vector2(1.0f / vcsWidth, 1.0f / vcsHeight));
            var texSize = new Vector2(vcsWidth, ScreenWidth / 1.28f /*- 168*/);
            _effect.Parameters["textureSize"]?.SetValue(texSize);
            _effect.Parameters["videoSize"]?.SetValue(texSize);
            var outSize = new Vector2(vcsHeight, ScreenHeight);
            _effect.Parameters["outputSize"]?.SetValue(outSize);
        }

        private Vector2 HandleInput(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                    return -Vector2.UnitY;
                case Keys.Down:
                case Keys.S:
                    return Vector2.UnitY;
                case Keys.Left:
                case Keys.A:
                    return -Vector2.UnitX;
                case Keys.Right:
                case Keys.D:
                    return Vector2.UnitX;
                default:
                    return Vector2.Zero;
            }
        }
    }
}
