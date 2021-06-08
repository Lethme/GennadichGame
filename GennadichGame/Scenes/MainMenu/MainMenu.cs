using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Enums;
using GennadichGame.Input;
using GennadichGame.Controls.Menu;

namespace GennadichGame.Scenes.MainMenu
{
    public sealed class MainMenu : Scene
    {
        private Menu _menu;
        public MainMenu(params MenuItem[] items)
        {
            _menu = new Menu(items);
            _menu.Initialize();
        }
        public MainMenu(Position position, Align textAlign, Fonts font, params MenuItem[] items)
        {
            _menu = new Menu(position, textAlign, font, items);
            _menu.Initialize(position, textAlign, font);
        }
        public override void Update(GameTime gameTime) => _menu.Update(gameTime);
        public override void Draw(GameTime gameTime) => _menu.Draw(gameTime);
    }
}
