using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OZ.MonoGame.MathFun.GameEngine;
using OZ.MonoGame.MathFun.GameEngine.Cards;
using OZ.MonoGame.MathFun.GameObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OZ.MonoGame.MathFun.Games;
using OZ.MonoGame.GameObjects.UI;
using System;
using OZ.MonoGame.MathFun.GameObjects.Effects;
using OZ.MonoGame.GameObjects.Effects;
using OZ.MonoGame;
using OZ.MonoGame.MathFun.GameObjects.UI;

namespace OZ.MonoGame.MathFun
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : GamePrototype
    {

        const double PANEL_TIME_TO_LEAVE = 500;

        MemoryGameControl _memoryGameControl;

        MainMenu _mainMenu;
        LevelsMenu _chooseLevel;
        PlayersNameMenu _playersNameMenu;

        Stack<GameMenu> _menuesStack;

        public SpriteFont Font { get; private set; }
        public Vector2 Size => new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);
        public Game(int width = 0, int height = 0)
        {
            SuspendLayout();

#if ANDROID
            Graphics = new GraphicsDeviceManager(this)
            {
            };
            Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = width;  // set this value to the desired width of your window
            Graphics.PreferredBackBufferHeight = height;   // set this value to the desired height of your window

            Graphics.ApplyChanges();

#else
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)(500 * 1.5),  // set this value to the desired width of your window
                PreferredBackBufferHeight = (int)(400 * 1.5)   // set this value to the desired height of your window
            };
            Graphics.ApplyChanges();
            this.IsMouseVisible = true;
#endif

            Content.RootDirectory = "Content";


            _memoryGameControl = new MemoryGameControl(this, Graphics);
            _menuesStack = new Stack<GameMenu>();

            InitMenues();
        }

        private void InitMenues()
        {
            _mainMenu = new MainMenu(this);

            _chooseLevel = new LevelsMenu(this, 3)
            {
                IsEnabled = false,
                IsVisible = false
            };

            _playersNameMenu = new PlayersNameMenu(this, 2)
            {
                IsEnabled = false,
                IsVisible = false
            };

            Controls.AddRange(new GameMenu[] { _mainMenu, _chooseLevel, _playersNameMenu });

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _memoryGameControl.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            LoadUIThings();

            base.LoadContent();

            _memoryGameControl.LoadContent(Content);
            InitMainMenu();
            InitLevels();
            InitPlayersNameMenu();
            ResumeLayout();
        }

        private void LoadUIThings()
        {
            UIApearance.BtnReg = Content.Load<Texture2D>("UI/sprites/regBtn");
            UIApearance.BtnHovered = Content.Load<Texture2D>("UI/sprites/hoverBtn");
            UIApearance.BtnPressed = Content.Load<Texture2D>("UI/sprites/pressedBtn");

            LoadTextBoxTextures();

            UIApearance.MenuBkg = Content.Load<Texture2D>("background/menuBkg");
            UIApearance.Font = Font = Content.Load<SpriteFont>(@"fonts/cardTextFont");
        }

        private void LoadTextBoxTextures()
        {
            Texture2D reg = UIApearance.TextBoxReg = Content.Load<Texture2D>("UI/sprites/TextBoxReg");
            int width = reg.Width;
            int height = reg.Height;

            var graphicsDevice = Graphics.GraphicsDevice;
            RenderTarget2D glowTextBox = new RenderTarget2D(
                graphicsDevice,
                graphicsDevice.PresentationParameters.BackBufferWidth,
                graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            graphicsDevice.SetRenderTarget(glowTextBox);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            var spriteBatch = SpriteBatch;
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                              null,
                              null,
                              null,
                              null,
                              Content.Load<Effect>("Effects/UI.GlowingEffect"),
                              null);
            spriteBatch.Draw(reg, Vector2.Zero, Color.White);
            spriteBatch.End();

            // Drop the render target
            graphicsDevice.SetRenderTarget(null);

            UIApearance.TextBoxHovered = UIApearance.TextBoxPressed = glowTextBox;
        }

        private void InitMainMenu()
        {
            _mainMenu.ToMiddle();

            _mainMenu.StartGameBtnClicked += (sender, e) => ToNextMenu(_chooseLevel);
            _mainMenu.ExitBtnClicked += (sender, e) =>
            {
                Exit();
            };
            _menuesStack.Push(_mainMenu);

        }

        int _level = 0;
        private void InitLevels()
        {
            _chooseLevel.ToMiddle();

            _chooseLevel.LevelChoosed += (sender, lvl) =>
            {
                _level = lvl;
                ToNextMenu(_playersNameMenu);
            };

            _chooseLevel.BackClicked += (sender, e) => ToOldMenu();

        }

        private void InitPlayersNameMenu()
        {
            _playersNameMenu.ToMiddle();


            _playersNameMenu.StartClicked += (sender, e) =>
            {

                StartGame();
            };

            _playersNameMenu.BackClicked += (sender, e) => ToOldMenu();

        }

        //Start animation that replace the current menu with the next.
        private void ToNextMenu(GameMenu nextGameMenu)
        {
            GameMenu oldGameMenu = _menuesStack.Peek();
            ReplaceMenus animate = new ReplaceMenus(oldGameMenu, nextGameMenu)
            {
                TimeToReach = PANEL_TIME_TO_LEAVE,
                WindowSize = Size,
                AnimateDirection = Direction.Up
            };
            animate.ActionInEnd += () =>
            {
                _menuesStack.Push(nextGameMenu);
                Animations.Remove(animate);
            };

            Animations.Add(animate);
            animate.StartAnimate();
        }

        private void StartGame()
        {
            var currentMenu = _menuesStack.Peek();
            var removeMenuAnimation = new RemoveMenuAnimate(currentMenu)
            {
                TimeToReach = PANEL_TIME_TO_LEAVE,
            };
            removeMenuAnimation.Destination = new Vector2()
            {
                Y = currentMenu.Location.Y,
                X = -currentMenu.Size.X
            };

            removeMenuAnimation.ActionInEnd += () =>
            {
                currentMenu.IsEnabled = currentMenu.IsVisible = false;

                _memoryGameControl.AssignPlayersName(_playersNameMenu[0], _playersNameMenu[1]);
                _memoryGameControl.StartNewGame(_level);
                Animations.Remove(removeMenuAnimation);
            };

            Animations.Add(removeMenuAnimation);
            removeMenuAnimation.StartAnimate();

        }

        //Start animation that replace the current menu with the old one.
        private void ToOldMenu()
        {
            GameMenu currentGameMenu = _menuesStack.Pop();
            GameMenu oldGameMenu = _menuesStack.Peek();
            ReplaceMenus animate = new ReplaceMenus(currentGameMenu, oldGameMenu)
            {
                TimeToReach = PANEL_TIME_TO_LEAVE,
                WindowSize = Size,
                AnimateDirection = Direction.Down
            };
            animate.ActionInEnd += () =>
            {
                Animations.Remove(animate);
            };

            Animations.Add(animate);
            animate.StartAnimate();
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _memoryGameControl.UnloadContent(Content);
            Content.Unload();
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            _memoryGameControl.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(new Color(231, 173, 81));
            base.Draw(gameTime);
        }

        protected override void InDraw(GameTime gameTime)
        {
            _memoryGameControl.Draw(gameTime, SpriteBatch);
#if DEBUG
            DrawCoordinates(SpriteBatch);
#endif
            base.InDraw(gameTime);

        }

        private void DrawCoordinates(SpriteBatch spriteBatch)
        {
            Point point = Mouse.GetState().Position;


            SpriteBatch.DrawString(ScoreBar.Font,
                                    point.X + "," + point.Y,
                                    Vector2.Zero,
                                    Color.Blue);

            SpriteBatch.DrawString(ScoreBar.Font,
                        point.X + "," + point.Y,
                        Vector2.Zero,
                        Color.Blue);
        }
    }
}
