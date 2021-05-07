using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GennadichGame
{
    public enum GameState
    {
        MainMenu,
        GameLobby,
        Game,
        Score
    }
    public class Keyboard
    {
        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        public static KeyboardState GetState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            return currentKeyState;
        }

        public static bool IsPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
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

        private GameState _state = GameState.MainMenu;

        private MainMenu _mainMenu;
        public GennadichGame(int width, int height)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _windowSize = new Point(width, height);
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
            
            _board = Content.Load<Texture2D>("board");
            _boardScale = 0.3f;
            _boardPosition = new Vector2(Window.ClientBounds.Width / 2 - _board.Width * _boardScale / 2, Window.ClientBounds.Height / 2 - _board.Height * _boardScale / 2);

            _fontSprite = Content.Load<SpriteFont>("consolas16");

            _mainMenu = new MainMenu(Window, _graphics, _spriteBatch, _fontSprite,
                new MainMenuItem("Play offline", () => { }),
                new MainMenuItem("Create game", () => { }),
                new MainMenuItem("Connect to existing game", () => { }),
                new MainMenuItem("Exit", () => Exit())
            );
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //var keyboardState = Keyboard.GetState();
            if (Keyboard.HasBeenPressed(Keys.Up))
            {
                _mainMenu.SelectedItemIndex -= 1;
            }
            if (Keyboard.HasBeenPressed(Keys.Down))
            {
                _mainMenu.SelectedItemIndex += 1;
            }
            if (Keyboard.HasBeenPressed(Keys.Enter))
            {
                _mainMenu.SelectedItem.Action.Invoke();
            }

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

            _spriteBatch.Begin();
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        private void UpdateMainMenu(GameTime gameTime)
        {

        }
        private void DrawMainMenu(GameTime gameTime)
        {
            _mainMenu.Draw();
        }
    }
}
