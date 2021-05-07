using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GennadichGame
{
    public class MainMenuItem
    {
        public String Text { get; set; }
        public Point Size { get; set; }
        public Rectangle Rect { get; set; }
        public Action Action { get; set; }
        public MainMenuItem(String text, Action action)
        {
            this.Text = text;
            this.Action = action;
        }
    }
    public class MainMenu
    {
        private GameWindow _window;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private List<MainMenuItem> _items;
        private Point _mousePosition;
        private Texture2D _selectedItemRect;
        private Color _selectedItemRectColor = Color.AliceBlue;
        private int _selectedItemIndex = 0;
        private Vector2 _center;
        private float _itemHeight;
        private float _maxItemWidth;
        private MainMenuItem SelectedItem => _items[_selectedItemIndex];
        private int SelectedItemIndex 
        { 
            get
            {
                return _selectedItemIndex;
            }
            set
            {
                if (!(value < 0 || value > ItemsCount - 1)) _selectedItemIndex = value;
            }
        }
        public int ItemsCount => _items.Count;
        public MainMenu(GameWindow window, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont spriteFont, params MainMenuItem[] items)
        {
            _items = new List<MainMenuItem>();
            _window = window;
            _graphics = graphics;
            _spriteBatch = spriteBatch;
            _font = spriteFont;
            AddItem(items);

            _center = new Vector2(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2);
            _itemHeight = _font.MeasureString(_items[0].Text).Y;

            _maxItemWidth = 0;
            
            foreach (var item in _items)
            {
                var width = _font.MeasureString(item.Text).X;
                item.Size = new Point((int)width, (int)_itemHeight);
                if (width > _maxItemWidth) _maxItemWidth = width;
            }

            _maxItemWidth *= 1.2f;

            var position = new Vector2(_center.X, _center.Y - _items.Count / 2 * _itemHeight + _itemHeight / 2.5f);
            foreach (var item in _items)
            {
                item.Rect = new Rectangle(
                    (int)(position.X - _maxItemWidth / 2),
                    (int)(position.Y - _itemHeight / 2),
                    (int)_maxItemWidth,
                    (int)_itemHeight
                );

                position.Y += _itemHeight;
            }

            _selectedItemRect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _selectedItemRect.SetData(new[] { Color.CornflowerBlue });
        }
        public void AddItem(params MainMenuItem[] items)
        {
            foreach (var item in items) _items.Add(item);
        }
        public void Invoke() => SelectedItem.Action.Invoke();
        public void Update()
        {
            _mousePosition = Mouse.GetState().Position;

            var itemSelected = false;

            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Rect.Contains(_mousePosition))
                {
                    Mouse.SetCursor(MouseCursor.Hand);
                    SelectedItemIndex = i;
                    itemSelected = true;
                }
            }

            if (!itemSelected) Mouse.SetCursor(MouseCursor.Arrow);

            if (Keyboard.HasBeenPressed(Keys.Up))
            {
                SelectedItemIndex -= 1;
            }
            if (Keyboard.HasBeenPressed(Keys.Down))
            {
                SelectedItemIndex += 1;
            }
            if (Keyboard.HasBeenPressed(Keys.Enter))
            {
                Invoke();
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && itemSelected)
            {
                Invoke();
            }
        }
        public void Draw()
        {
            var position = new Vector2(_center.X, _center.Y - _items.Count / 2 * _itemHeight);

            _spriteBatch.Begin();
            //_spriteBatch.DrawString(_font, _items[0].Text, center - _font.MeasureString(_items[0].Text) / 2, Color.Red);

            if (SelectedItemIndex != -1)
            {
                var item = _items[SelectedItemIndex];
                _spriteBatch.Draw(_selectedItemRect, item.Rect, _selectedItemRectColor);
            }

            foreach (var item in _items)
            {
                _spriteBatch.DrawString(_font, item.Text, new Vector2(position.X - _font.MeasureString(item.Text).X / 2, position.Y), Color.Black);
                position.Y += _itemHeight;
            }

            _spriteBatch.End();
        }
    }
}
