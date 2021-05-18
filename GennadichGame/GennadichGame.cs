using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GennadichGame
{
    public class GennadichGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _dartsTexture;
        private SpriteFont _spriteFont;
        private Point _windowSize;
        private Texture2D _arrowCursorTex;
        private Texture2D _pointerCursorTex;
        private Texture2D _currentCursorTex;
        private Texture2D _backgroundTex;
        private GameState _gameState = GameState.MainMenu;
        private Cursor _currentCursor = Cursor.Arrow;
        private MainMenu _mainMenu;
        private GDarts _darts;
        public Dictionary<string, Texture2D> Backgrounds { get; }
        public GraphicsDeviceManager Graphics => _graphics;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public SpriteFont SpriteFont => _spriteFont;
        public Texture2D DartsTexture => _dartsTexture;
        public Vector2 CentralPoint { get; }
        public Texture2D Background
        {
            get { return _backgroundTex; }
            set { _backgroundTex = value; }
        }
        public Cursor CurrentCursor
        {
            get { return _currentCursor; }
            set { SetCursor(value); }
        }
        public GennadichGame(int width, int height)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _windowSize = new Point(width, height);

            CentralPoint = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

            Backgrounds = new Dictionary<string, Texture2D>();
        }
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _windowSize.X;
            _graphics.PreferredBackBufferHeight = _windowSize.Y;
            _graphics.ApplyChanges();

            Window.Title = "GDarts";

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _dartsTexture = Content.Load<Texture2D>("img/board");

            _spriteFont = Content.Load<SpriteFont>("font/consolas16");

            _arrowCursorTex = Content.Load<Texture2D>("img/arrow");
            _pointerCursorTex = Content.Load<Texture2D>("img/pointer");

            Backgrounds.Add("clouds", Content.Load<Texture2D>("img/background-1"));

            CurrentCursor = Cursor.Arrow;

            _mainMenu = new MainMenu(this, _spriteFont,
                new MainMenuItem("Play offline", 0, () => { }),
                new MainMenuItem("Create game", 0, () => { }),
                new MainMenuItem("Connect to existing game", 0, () => { }),
                new MainMenuItem("Exit", 0, () => Exit())
            );

            _darts = new GDarts(this, _dartsTexture);
        }
        protected override void Update(GameTime gameTime)
        {
            try
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (Keyboard.HasBeenPressed(Keys.F1)) _gameState = GameState.MainMenu;
                if (Keyboard.HasBeenPressed(Keys.F2)) _gameState = GameState.Game;

                switch (_gameState)
                {
                    case GameState.StartScreen:
                        {
                            break;
                        }
                    case GameState.MainMenu:
                        {
                            UpdateMainMenu(gameTime);
                            break;
                        }
                    case GameState.GameLobby:
                        {
                            break;
                        }
                    case GameState.Game:
                        {
                            UpdateGame(gameTime);
                            break;
                        }
                    case GameState.Score:
                        {
                            break;
                        }
                }

                base.Update(gameTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update method exception:\n\n{ex.Message}");
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.White);

                if (_backgroundTex != null)
                {
                    _spriteBatch.Begin();
                    _spriteBatch.Draw(_backgroundTex, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);
                    _spriteBatch.End();
                }

                switch (_gameState)
                {
                    case GameState.StartScreen:
                        {
                            break;
                        }
                    case GameState.MainMenu:
                        {
                            DrawMainMenu(gameTime);
                            break;
                        }
                    case GameState.GameLobby:
                        {
                            break;
                        }
                    case GameState.Game:
                        {
                            DrawGame(gameTime);
                            break;
                        }
                    case GameState.Score:
                        {
                            break;
                        }
                }

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Draw method exception:\n\n{ex.Message}");
            }
        }
        private void SetCursor(Texture2D cursorTexture)
        {
            if (_currentCursorTex != cursorTexture)
            {
                Mouse.SetCursor(MouseCursor.FromTexture2D(cursorTexture, 0, 0));
                _currentCursorTex = cursorTexture;
            }
        }
        public void SetCursor(Cursor cursor)
        {
            _currentCursor = cursor;
            switch (cursor)
            {
                case Cursor.Pointer:
                    {
                        SetCursor(_pointerCursorTex);
                        break;
                    }
                default:
                    {
                        SetCursor(_arrowCursorTex);
                        break;
                    }
            }
        }
        private void UpdateMainMenu(GameTime gameTime) => _mainMenu.Update();
        private void DrawMainMenu(GameTime gameTime) => _mainMenu.Draw();
        private void UpdateGame(GameTime gameTime) => _darts.Update();
        private void DrawGame(GameTime gameTime) => _darts.Draw();
    }
}
