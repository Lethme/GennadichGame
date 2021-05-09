using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GennadichGame
{
    public class GDarts
    {
        private GennadichGame _game;
        private Texture2D _dartsTex;
        private float _dartsScale;
        private Vector2 _dartsPosition;
        public GDarts(GennadichGame game, Texture2D dartsTex)
        {
            _game = game;
            _dartsTex = dartsTex;

            _dartsScale = 0.5f;
            _dartsPosition = new Vector2(
                _game.Window.ClientBounds.Width / 2 - _dartsTex.Width * _dartsScale / 2 + _game.Window.ClientBounds.Width * 0.2f,
                _game.Window.ClientBounds.Height / 2 - _dartsTex.Height * _dartsScale / 2
            );
        }
        public void Update()
        {

        }
        public void Draw()
        {
            _game.SpriteBatch.Begin();
            _game.SpriteBatch.Draw(_dartsTex, new Rectangle(_dartsPosition.ToPoint(), new Point((int)(_dartsTex.Width * _dartsScale), (int)(_dartsTex.Height * _dartsScale))), Color.White);
            _game.SpriteBatch.End();
        }
    }
}
