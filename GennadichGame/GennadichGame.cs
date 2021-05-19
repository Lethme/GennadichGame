﻿using System;
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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Point _windowSize;
        private MainMenu _mainMenu;
        private GDarts _darts;
        public TextureManager TextureManager { get; }
        public BackgroundManager BackgroundManager { get; }
        public CursorManager CursorManager { get; }
        private SceneManager SceneManager { get; }
        public GraphicsDeviceManager Graphics => _graphics;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public SpriteFont SpriteFont => _spriteFont;
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

            _mainMenu = new MainMenu(this,
                new MainMenuItem("Play offline", 0, () => { }),
                new MainMenuItem("Create game", 0, () => { }),
                new MainMenuItem("Connect to existing game", 0, () => { }),
                new MainMenuItem("Exit", 0, () => Exit())
            );

            _darts = new GDarts(this, TextureManager[Textures.Darts]);

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
                (GameState.MainMenu, _mainMenu),
                (GameState.Game, _darts)
            );

            CursorManager.ActiveCursor = Cursor.Dart;
            BackgroundManager.ActiveBackground = BackgroundImage.None;
            SceneManager.ActiveState = GameState.MainMenu;
        }
        protected override void Update(GameTime gameTime)
        {
            try
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || GKeyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (GKeyboard.HasBeenPressed(Keys.F1)) SceneManager.ActiveState = GameState.MainMenu;
                if (GKeyboard.HasBeenPressed(Keys.F2)) SceneManager.ActiveState = GameState.Game;

                SceneManager.ActiveScene.Update(gameTime);

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

                BackgroundManager.Draw();

                SceneManager.ActiveScene.Draw(gameTime);

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Draw method exception:\n\n{ex.Message}");
            }
        }
    }
}
