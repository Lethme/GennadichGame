using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;

namespace GennadichGame.Manager
{
    public class BackgroundManager
    {
        private GennadichGame _game;
        private BackgroundImage _currentBackground;
        private Texture2D _currentBackgroundTex;
        public BackgroundImage ActiveBackground
        {
            get { return _currentBackground; }
            set { SetActiveBackground(value); }
        }
        private Dictionary<BackgroundImage, Texture2D> Backgrounds { get; } = new Dictionary<BackgroundImage, Texture2D>();
        public BackgroundManager(GennadichGame game) 
        {
            _game = game;
        }
        public BackgroundManager(GennadichGame game, params KeyValuePair<BackgroundImage, Texture2D>[] backgrounds)
        {
            _game = game;
            AddBackground(backgrounds);
        }
        public BackgroundManager(GennadichGame game, params (BackgroundImage image, Texture2D backgroundTexture)[] backgrounds)
        {
            _game = game;
            AddBackground(backgrounds);
        }
        public void AddBackground(params KeyValuePair<BackgroundImage, Texture2D>[] backgrounds)
        {
            foreach (var background in backgrounds) Backgrounds.Add(background.Key, background.Value);
        }
        public void AddBackground(params (BackgroundImage image, Texture2D backgroundTexture)[] backgrounds)
        {
            foreach (var background in backgrounds) Backgrounds.Add(background.image, background.backgroundTexture);
        }
        public void Draw()
        {
            if (_currentBackgroundTex != null)
            {
                _game.SpriteBatch.Begin();
                _game.SpriteBatch.Draw(_currentBackgroundTex, new Rectangle(0, 0, _game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height), Color.White);
                _game.SpriteBatch.End();
            }
        }
        public void SetActiveBackground(BackgroundImage image)
        {
            if (SetActiveBackground(Backgrounds[image])) _currentBackground = image;
        }
        private bool SetActiveBackground(Texture2D backgroundTexture)
        {
            if (_currentBackgroundTex != backgroundTexture)
            {
                _currentBackgroundTex = backgroundTexture;
                return true;
            }

            return false;
        }
    }
}
