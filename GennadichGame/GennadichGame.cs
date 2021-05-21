using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Input;
using GennadichGame.Enums;
using GennadichGame.Manager;
using GennadichGame.Scenes.Menu;
using GennadichGame.Scenes.Darts;

namespace GennadichGame
{
    public class GennadichGame : Game
    {
        #region Data
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Point _windowSize;
        #endregion
        #region Managers
        public TextureManager TextureManager { get; }
        public BackgroundManager BackgroundManager { get; }
        public CursorManager CursorManager { get; }
        private SceneManager SceneManager { get; }
        #endregion
        #region Properties
        public GraphicsDeviceManager Graphics => _graphics;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public SpriteFont SpriteFont => _spriteFont;
        #endregion
        #region Constructors
        public GennadichGame(int width, int height)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _windowSize = new Point(width, height);

            BackgroundManager = new BackgroundManager(this);
            TextureManager = new TextureManager();
            CursorManager = new CursorManager();
            SceneManager = new SceneManager();
        }
        #endregion
        #region ProtectedMethods
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

            TextureManager.AddTexture
            (
                (Textures.Darts, Content.Load<Texture2D>("img/board")),
                (Textures.ArrowCursor, Content.Load<Texture2D>("img/arrow")),
                (Textures.PointerCursor, Content.Load<Texture2D>("img/pointer")),
                (Textures.DartCursor, Content.Load<Texture2D>("img/dart")),
                (Textures.Background1, Content.Load<Texture2D>("img/background-1"))
            );

            _spriteFont = Content.Load<SpriteFont>("font/consolas16");

            CursorManager.AddCursor
            (
                (Cursor.Arrow, TextureManager[Textures.ArrowCursor]),
                (Cursor.Pointer, TextureManager[Textures.PointerCursor]),
                (Cursor.Dart, TextureManager[Textures.DartCursor])
            );

            BackgroundManager.AddBackground
            (
                (BackgroundImage.None, null),
                (BackgroundImage.Clouds, TextureManager[Textures.Background1])
            );

            SceneManager.AddScene
            (
                (GameState.MainMenu, new MainMenu(this,
                    ("Play offline", 0, () => { }),
                    ("Create game", 0, () => { }),
                    ("Connect to existing game", 0, () => { }),
                    ("Exit", 0, () => Exit())
                )),
                (GameState.Game, new GDarts(this, TextureManager[Textures.Darts]))
            );

            SceneManager[GameState.MainMenu].OnActivate += () =>
            {
                BackgroundManager.ActiveBackground = BackgroundImage.Clouds;
                CursorManager.ActiveCursor = Cursor.Dart;
            };

            SceneManager[GameState.Game].OnActivate += () =>
            {
                BackgroundManager.ActiveBackground = BackgroundImage.Clouds;
                CursorManager.ActiveCursor = Cursor.Dart;
            };

            CursorManager.ActiveCursor = Cursor.Dart;
            BackgroundManager.ActiveBackground = BackgroundImage.None;
            SceneManager.ActiveState = GameState.MainMenu;
        }
        protected override void Update(GameTime gameTime)
        {
            try
            {
                GKeyboard.UpdateState();
                GMouse.UpdateState();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GKeyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (GKeyboard.IsKeyPressed(Keys.F1)) SceneManager.ActiveState = GameState.MainMenu;
                if (GKeyboard.IsKeyPressed(Keys.F2)) SceneManager.ActiveState = GameState.Game;
                if (GKeyboard.IsKeyPressed(Keys.A))
                {
                    if (GMouse.AlkashCursor) GMouse.AlkashCursor = false;
                    else GMouse.AlkashCursor = true;
                }

                SceneManager.ActiveScene.Update(gameTime);

                base.Update(gameTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update method exception:\n{ex.Message}");
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.White);

                BackgroundManager.DrawBackground();

                SceneManager.ActiveScene.Draw(gameTime);

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Draw method exception:\n{ex.Message}");
            }
        }
        #endregion
    }
}
