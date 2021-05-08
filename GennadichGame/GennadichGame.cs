using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace GennadichGame
{
    public enum GameState
    {
        MainMenu,
        GameLobby,
        Game,
        Score
    }
    public class GennadichGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _board;
        private SpriteFont _fontSprite;
        private Vector2 _boardPosition;
        private float _boardScale;
        private Point _windowSize;
        private Texture2D _arrowCursorTex;
        private Texture2D _pointerCursorTex;
        private Texture2D _backgroundTex;
        private GameState _state = GameState.MainMenu;
        private MainMenu _mainMenu;
        public Dictionary<string, Texture2D> Backgrounds { get; }
        public Texture2D ArrowCursorTex => _arrowCursorTex;
        public Texture2D PointerCursorTex => _pointerCursorTex;
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
            
            _board = Content.Load<Texture2D>("img/board");
            _boardScale = 0.3f;
            _boardPosition = new Vector2(Window.ClientBounds.Width / 2 - _board.Width * _boardScale / 2, Window.ClientBounds.Height / 2 - _board.Height * _boardScale / 2);

            _fontSprite = Content.Load<SpriteFont>("font/consolas16");

            _arrowCursorTex = Content.Load<Texture2D>("img/arrow");
            _pointerCursorTex = Content.Load<Texture2D>("img/pointer");

            Backgrounds.Add("clouds", Content.Load<Texture2D>("img/background-1"));

            Mouse.SetCursor(MouseCursor.FromTexture2D(_arrowCursorTex, 0, 0));

            _mainMenu = new MainMenu(this, _graphics, _spriteBatch, _fontSprite,
                new MainMenuItem("Play offline", 0, () => { }),
                new MainMenuItem("Create game", 0, () => { }),
                new MainMenuItem("Connect to existing game", 0, () => { }),
                new MainMenuItem("Exit", 0, () => Exit())
            );
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_state)
            {
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

            switch (_state)
            {
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
    }
}
