using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Input;
using GennadichGame.Enums;
using GennadichGame.Scenes;
using GennadichGame.Scenes.Menu;
using GennadichGame.Scenes.Darts;

namespace GennadichGame
{
    public class GennadichGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _dartsTexture;
        private SpriteFont _spriteFont;
        private Point _windowSize;
        private Texture2D _currentCursorTex;
        private Texture2D _backgroundTex;
        private GameState _gameState;
        private Cursor _currentCursor;
        private BackgroundImage _currentBackground = BackgroundImage.Clouds;
        private MainMenu _mainMenu;
        private GDarts _darts;
        public Dictionary<BackgroundImage, Texture2D> Backgrounds { get; }
        private Dictionary<Cursor, Texture2D> Cursors { get; }
        private Dictionary<GameState, Scene> Scenes { get; }
        public GraphicsDeviceManager Graphics => _graphics;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public SpriteFont SpriteFont => _spriteFont;
        public Texture2D DartsTexture => _dartsTexture;
        public Vector2 CentralPoint { get; }
        public Scene ActiveScene => Scenes.Values.FirstOrDefault(scene => scene.Active);
        public GameState CurrentScene
        {
            get { return _gameState; }
            set { SetActiveScene(value); }
        }
        public BackgroundImage CurrentBackground
        {
            get { return _currentBackground; }
            set { SetBackground(value); }
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

            Backgrounds = new Dictionary<BackgroundImage, Texture2D>();
            Cursors = new Dictionary<Cursor, Texture2D>();
            Scenes = new Dictionary<GameState, Scene>();
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
            
            Cursors.Add(Cursor.Arrow, Content.Load<Texture2D>("img/arrow"));
            Cursors.Add(Cursor.Pointer, Content.Load<Texture2D>("img/pointer"));
            Cursors.Add(Cursor.Dart, Content.Load<Texture2D>("img/dart"));

            Backgrounds.Add(BackgroundImage.None, null);
            Backgrounds.Add(BackgroundImage.Clouds, Content.Load<Texture2D>("img/background-1"));

            _mainMenu = new MainMenu(this, _spriteFont,
                new MainMenuItem("Play offline", 0, () => { }),
                new MainMenuItem("Create game", 0, () => { }),
                new MainMenuItem("Connect to existing game", 0, () => { }),
                new MainMenuItem("Exit", 0, () => Exit())
            );

            _darts = new GDarts(this, _dartsTexture);

            Scenes.Add(GameState.MainMenu, _mainMenu);
            Scenes.Add(GameState.Game, _darts);

            CurrentCursor = Cursor.Dart;
            CurrentBackground = BackgroundImage.None;
            CurrentScene = GameState.MainMenu;
        }
        protected override void Update(GameTime gameTime)
        {
            try
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GKeyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (GKeyboard.HasBeenPressed(Keys.F1)) CurrentScene = GameState.MainMenu;
                if (GKeyboard.HasBeenPressed(Keys.F2)) CurrentScene = GameState.Game;

                ActiveScene.Update(gameTime);

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

                ActiveScene.Draw(gameTime);

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Draw method exception:\n\n{ex.Message}");
            }
        }
        private void SetBackground(Texture2D backgroundTexture)
        {
            if (_backgroundTex != backgroundTexture)
            {
                _backgroundTex = backgroundTexture;
            }
        }
        private void SetBackground(BackgroundImage image)
        {
            _currentBackground = image;
            SetBackground(Backgrounds[image]);
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
            SetCursor(Cursors[cursor]);
        }
        public void SetActiveScene(GameState scene)
        {
            _gameState = scene;
            if (ActiveScene != null)
            {
                ActiveScene.Deactivate();
                ActiveScene.Active = false;
            }
            foreach (var sc in Scenes)
            {
                if (sc.Key == scene)
                {
                    sc.Value.Active = true;
                    sc.Value.Activate();
                }
            }
        }
    }
}
