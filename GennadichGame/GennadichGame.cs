using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Input;
using GennadichGame.Enums;
using GennadichGame.Scenes;
using GennadichGame.Manager;
using GennadichGame.Scenes.Menu;
using GennadichGame.Scenes.Darts;
using GennadichGame.Scenes.Lobby;

namespace GennadichGame
{
    public class GennadichGame : Game
    {
        #region Data
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Point _windowSize;
        #endregion
        #region Managers
        public TextureManager TextureManager { get; }
        public FontManager FontManager { get; }
        public BackgroundManager BackgroundManager { get; }
        public CursorManager CursorManager { get; }
        private SceneManager SceneManager { get; }
        #endregion
        #region Properties
        public GraphicsDeviceManager Graphics => _graphics;
        public SpriteBatch SpriteBatch => _spriteBatch;
        #endregion
        #region Constructors
        public GennadichGame(int width, int height)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _windowSize = new Point(width, height);

            BackgroundManager = new BackgroundManager(this);
            FontManager = new FontManager();
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

            Scene.Initialize(this);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.AddTexture
            (
                (Textures.Dart, Content.Load<Texture2D>("img/flying_dart")),
                (Textures.Darts, Content.Load<Texture2D>("img/board")),
                (Textures.ArrowCursor, Content.Load<Texture2D>("img/arrow")),
                (Textures.PointerCursor, Content.Load<Texture2D>("img/pointer")),
                (Textures.DartCursor, Content.Load<Texture2D>("img/dart")),
                (Textures.Background1, Content.Load<Texture2D>("img/background-1")),
                (Textures.Background2, Content.Load<Texture2D>("img/background-2"))
            );

            FontManager.AddFont
            (
                (Fonts.RegularConsolas8, Content.Load<SpriteFont>("font/consolas/regular/8")),
                (Fonts.RegularConsolas10, Content.Load<SpriteFont>("font/consolas/regular/10")),
                (Fonts.RegularConsolas12, Content.Load<SpriteFont>("font/consolas/regular/12")),
                (Fonts.RegularConsolas14, Content.Load<SpriteFont>("font/consolas/regular/14")),
                (Fonts.RegularConsolas16, Content.Load<SpriteFont>("font/consolas/regular/16")),
                (Fonts.RegularConsolas18, Content.Load<SpriteFont>("font/consolas/regular/18")),
                (Fonts.RegularConsolas20, Content.Load<SpriteFont>("font/consolas/regular/20")),
                (Fonts.RegularConsolas22, Content.Load<SpriteFont>("font/consolas/regular/22")),
                (Fonts.RegularConsolas24, Content.Load<SpriteFont>("font/consolas/regular/24")),
                (Fonts.RegularConsolas26, Content.Load<SpriteFont>("font/consolas/regular/26")),
                (Fonts.RegularConsolas28, Content.Load<SpriteFont>("font/consolas/regular/28")),
                (Fonts.RegularConsolas30, Content.Load<SpriteFont>("font/consolas/regular/30")),
                (Fonts.RegularConsolas32, Content.Load<SpriteFont>("font/consolas/regular/32")),
                (Fonts.RegularConsolas40, Content.Load<SpriteFont>("font/consolas/regular/40")),
                (Fonts.RegularConsolas48, Content.Load<SpriteFont>("font/consolas/regular/48")),
                (Fonts.RegularConsolas56, Content.Load<SpriteFont>("font/consolas/regular/56")),
                (Fonts.RegularConsolas64, Content.Load<SpriteFont>("font/consolas/regular/64")),
                (Fonts.RegularConsolas72, Content.Load<SpriteFont>("font/consolas/regular/72")),
                (Fonts.RegularConsolas80, Content.Load<SpriteFont>("font/consolas/regular/80"))
            );

            CursorManager.AddCursor
            (
                (Cursor.Arrow, TextureManager[Textures.ArrowCursor]),
                (Cursor.Pointer, TextureManager[Textures.PointerCursor]),
                (Cursor.Dart, TextureManager[Textures.DartCursor])
            );

            BackgroundManager.AddBackground
            (
                (BackgroundImage.None, null),
                (BackgroundImage.Clouds, TextureManager[Textures.Background1]),
                (BackgroundImage.Evening, TextureManager[Textures.Background2])
            );

            SceneManager.AddScene
            (
                (GameState.MainMenu, new MainMenu
                (
                    ("Play offline", 0, () => { }),
                    ("Create game", 0, () => { }),
                    ("Connect to existing game", 0, () => { }),
                    ("Exit", 0, () => Exit())
                )),
                (GameState.Game, new GDarts(TextureManager[Textures.Darts])),
                (GameState.GameLobby, new GameLobby())
            );

            SceneManager[GameState.MainMenu].OnActivate += (s) =>
            {
                BackgroundManager.ActiveBackground = BackgroundImage.Clouds;
                CursorManager.ActiveCursor = Cursor.Dart;
            };

            SceneManager[GameState.Game].OnActivate += (s) =>
            {
                BackgroundManager.ActiveBackground = BackgroundImage.Clouds;
                CursorManager.ActiveCursor = Cursor.Dart;
            };

            SceneManager[GameState.GameLobby].OnActivate += (s) =>
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

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GKeyboard.IsKeyDown(Keys.Escape))
                    Exit();

                if (GKeyboard.IsKeyPressed(Keys.F1)) SceneManager.ActiveState = GameState.MainMenu;
                if (GKeyboard.IsKeyPressed(Keys.F2)) SceneManager.ActiveState = GameState.GameLobby;
                if (GKeyboard.IsKeyPressed(Keys.F3)) SceneManager.ActiveState = GameState.Game;
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
