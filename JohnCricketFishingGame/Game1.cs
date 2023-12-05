using JohnCricketFishingGame.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input.InputListeners;
using System;
using System.ComponentModel;

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
        private GameStats _gameStats;
        private GameInput gameInput;
        private Rod _rod;
        private Effect _effect;
        private Menu[] _menus;
        private int _playerFishID = 0;
        private int _fishCount;
        

        private int vcsWidth = 192;
        private int vcsHeight = 160;
        private int ScreenWidth = 1024;
        private int ScreenHeight = 768;
        private float _startingDelay = 0f;
        private const float _maxStartingDelay = 1.2f;
        // bad/shortcut solution for one shot event input
        private bool _isOneShotInput = false;
        private Vector2 destination = Vector2.Zero;
        private KeyboardState _oldState;

        public static EventHandler<int> ScoreListener;
        public static bool IsPaused;
        public enum GameState { TitleScreen, Tutorial, GameOver, GameOrPauseScreen }
        public GameState CurrentGameState = GameState.TitleScreen;
        public static SaveData save;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            IsMouseVisible = true;


            GameContent = Content;
            GameGraphics = _graphics;

            gameInput = new GameInput();
           
            //This is not working as intended that's why it's commented out
            // Components.Add(new InputListenerComponent(this, gameInput.keyboardListener, gameInput.gamePadListener));

        }

        protected override void Initialize()
        {
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
            save = new SaveData();
            _effect = Content.Load<Effect>("Assets/Shaders/crt-lottes-mg");
            SetupEffect();
            AudioSystem.Instance.Play(AudioSystem.SongCollection.TitleTheme);
            SetupGame();
        }

       

        private void SetupGame()
        {
            Fish.fishList.Clear();

            _gameStats = new GameStats();

            //_gameStats.SetChallengeLevel();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fishCount = 8;
            _menus = new Menu[4];
            _menus[(int) GameState.TitleScreen] = new MenuTitleScreen();
            _menus[(int) GameState.Tutorial] = new MenuTutorial();
            _menus[(int) GameState.GameOver] = new MenuGameOver();
            _menus[(int) GameState.GameOrPauseScreen] = new Menu();

            _rod = new Rod();

            Fish.AddFishs(_fishCount);


            //gameInput.keyboardListener.KeyPressed += (sender, args) => destination = gameInput.HandleInput(args.Key); //{ Window.Title = $"Key {args.Key} Pressed"; };
           // gameInput.gamePadListener.ButtonDown += (sender, args) => { Window.Title = $"Key {args.Button} Down"; };

            ScoreListener += (sender, args) => 
            { 
                _rod.ResetRod(); 
                _playerFishID = 0; 
                Fish.UpdateFishStatus(); 
            };


            IsPaused = true;
        }


        protected override void Update(GameTime gameTime)
        {
            if (_startingDelay < _maxStartingDelay)
            {
                _startingDelay += gameTime.GetElapsedSeconds();
                return;
            }

            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed ||
                currentKeyboardState.IsKeyDown(Keys.P) && _oldState.IsKeyUp(Keys.P))
            {
                IsPaused = !IsPaused;
                
                if (IsPaused)
                {
                    AudioSystem.Instance.Pause();
                }
            }

            if ((GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) ||
               (currentKeyboardState.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space)))
            {
                if(CurrentGameState == GameState.TitleScreen)
                {
                    SetupGame();
                    MenuTitleScreen.DoOnceIsSet = true;
                    CurrentGameState = GameState.Tutorial;
                }
                else if(CurrentGameState == GameState.Tutorial)
                {
                    IsPaused = false;
                    CurrentGameState = GameState.GameOrPauseScreen;
                    AudioSystem.Instance.Play(AudioSystem.SFXCollection.Reset);
                }
                else if(CurrentGameState == GameState.GameOver)
                {
                    CurrentGameState = GameState.TitleScreen;
                    IsPaused = true;
                    _gameStats.ResetGame();
                    SetupGame();
                }
            }

            _oldState = currentKeyboardState;

            if (CurrentGameState < GameState.GameOrPauseScreen)
            {
                _menus[(int)CurrentGameState].Update(gameTime);
            }

            if (IsPaused == true)
            {
                return;
            }

            AudioSystem.Instance.Update();

            if(_gameStats.IsGameOver)
            {
                CurrentGameState = GameState.GameOver;
                _menus[(int) CurrentGameState].Update(gameTime);
            }

            _gameStats.Update(gameTime);

            _rod.Update(gameTime);

            // Only move fish if primary button is being hold
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Space))
            {
                if(currentKeyboardState.GetPressedKeys().Length > 1) 
                {
                    destination = gameInput.HandleInput(currentKeyboardState.GetPressedKeys()[1]);
                }
                //Customer awareness here
                _gameStats.IncreaseCustomerSuspicion(gameTime);
            }
            //changes highlit fish according to player input
            else
            {
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ||
                   currentKeyboardState.IsKeyDown(Keys.Left))
                {
                    ToggleFishControl(Keys.Left);
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
                    currentKeyboardState.IsKeyDown(Keys.Right))
                {
                    ToggleFishControl(Keys.Right);
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed ||
                  currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    ToggleFishControl(Keys.Up);
                }
                else if(GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed ||
                  currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    ToggleFishControl(Keys.Down);
                }
                else
                {
                    _isOneShotInput = false;
                }

                _gameStats.LowerSuspicion(gameTime);
                destination = Vector2.Zero;
            }
            

            for (int i = 0; i < Fish.fishList.Count; i++)
            {
                Fish.fishList[i].Update(gameTime);
            }

            if (Fish.fishList.Count > 0)
            {
                Fish.fishList[_playerFishID].Move(destination * gameTime.GetElapsedSeconds());
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
            
            DrawGame();

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

        private void DrawGame()
        {
            if (CurrentGameState < GameState.GameOrPauseScreen)
            {
                _menus[(int)CurrentGameState].Draw(_spriteBatch);
                return;
            }

            for (int i = 0; i < Fish.fishList.Count; i++)
            {
                Fish.fishList[i].Draw(_spriteBatch);
            }

            _rod.Draw(_spriteBatch);

            _gameStats.Draw(_spriteBatch);
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

        private void ToggleFishControl(Keys key)
        {
            _fishCount = Fish.fishList.Count;
            
            // this updates middle index whenever there is a different amount of fish rather than the starting 8
            int fishMidleIndex = ((_fishCount - 1) / 2);

            if (_isOneShotInput == true)
            {
                return;
            }

            _isOneShotInput = true;

            switch (key)
            {
                case Keys.Up:
                    if (_playerFishID > ((_fishCount - 1) / 2))
                    {
                        _playerFishID -= fishMidleIndex;
                    }
                    break;
                case Keys.Left:

                    if (_playerFishID > 0 /*&& _playerFishID != (_fishCount / 2)*/)
                    {
                        _playerFishID--;
                    }
                    break;
                case Keys.Down:
                    if (_playerFishID <= ((_fishCount - 1) / 2))
                    {
                        _playerFishID += fishMidleIndex;
                    }

                    break;
                case Keys.Right:

                    if (_playerFishID < (_fishCount - 1)/* && _playerFishID != ((_fishCount - 1) / 2)*/)
                    {
                        _playerFishID++;
                    }

                    break;
                default:
                    break;

            }

            Fish.UpdateFishStatus(_playerFishID);
        }

    }
}
