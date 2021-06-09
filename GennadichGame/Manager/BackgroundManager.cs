using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;
using GennadichGame.Controls;

namespace GennadichGame.Manager
{
    public class BackgroundManager : GameModule, IEnumerable<KeyValuePair<BackgroundImage, Texture2D>>
    {
        private BackgroundImage _currentBackground;
        private Texture2D _currentBackgroundTex;
        public BackgroundImage ActiveBackground
        {
            get { return _currentBackground; }
            set { SetActiveBackground(value); }
        }
        private Dictionary<BackgroundImage, Texture2D> Backgrounds { get; } = new Dictionary<BackgroundImage, Texture2D>();
        public BackgroundManager() { }
        public BackgroundManager(params KeyValuePair<BackgroundImage, Texture2D>[] backgrounds)
        {
            AddBackground(backgrounds);
        }
        public BackgroundManager(params (BackgroundImage image, Texture2D backgroundTexture)[] backgrounds)
        {            
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
        public void DrawBackground()
        {
            if (_currentBackgroundTex != null)
            {
                Game.SpriteBatch.Begin();
                Game.SpriteBatch.Draw(_currentBackgroundTex, new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height), Color.White);
                Game.SpriteBatch.End();
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
        public IEnumerator<KeyValuePair<BackgroundImage, Texture2D>> GetEnumerator()
        {
            return Backgrounds.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Backgrounds.GetEnumerator();
        }
    }
}
