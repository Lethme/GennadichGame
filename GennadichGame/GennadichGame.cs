using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace GennadichGame
{
    public enum GameState
    {
        StartScreen,
        MainMenu,
        GameLobby,
        Game,
        Score
    }
    public class GennadichGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _dartsTexture;
        private SpriteFont _spriteFont;
        private Point _windowSize;
        private Texture2D _arrowCursorTex;
        private Texture2D _pointerCursorTex;
        private Texture2D _backgroundTex;
        private GameState _gameState = GameState.MainMenu;
        private MainMenu _mainMenu;
        private GDarts _darts;
        public Dictionary<string, Texture2D> Backgrounds { get; }
        public GraphicsDeviceManager Graphics => _graphics;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public SpriteFont SpriteFont => _spriteFont;
        public Texture2D ArrowCursorTex => _arrowCursorTex;
        public Texture2D PointerCursorTex => _pointerCursorTex;
        public Texture2D DartsTexture => _dartsTexture;
        public Vector2 CentralPoint { get; }
        public Texture2D Background
        {
            get { return _backgroundTex; }
            set { _backgroundTex = value; }
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

            Mouse.SetCursor(MouseCursor.FromTexture2D(_arrowCursorTex, 0, 0));

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
        protected override void Draw(GameTime gameTime)
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
        private void UpdateMainMenu(GameTime gameTime)
        {
            _mainMenu.Update();
        }
        private void DrawMainMenu(GameTime gameTime)
        {
            _mainMenu.Draw();
        }
        private void UpdateGame(GameTime gameTime)
        {
            
        }
        private void DrawGame(GameTime gameTime)
        {
            _darts.Draw();
        }
    }
}
