using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Input;
using GennadichGame.Enums;

namespace GennadichGame.Controls.Menu
{
    public class Menu : Control
    {
        private List<MenuItem> _items;
        private Point _mousePosition;
        private Texture2D _selectedItemRect;
        private Color _selectedItemRectColor = Color.CornflowerBlue;
        private int _selectedItemIndex = 0;
        private bool _itemSelected;
        private Vector2 _center;
        private float _itemHeight;
        private float _maxItemWidth;
        private Position _menuPosition;
        private Align _menuTextAlign;
        private Fonts _menuFont;
        private MenuItem SelectedItem => _items[_selectedItemIndex];
        private int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set { if (!(value < 0 || value > ItemsCount - 1)) _selectedItemIndex = value; }
        }
        public Position Position 
        { 
            get { return _menuPosition; }
            set { _menuPosition = value; Initialize(_menuPosition, _menuTextAlign); }
        }
        public Align TextAlign
        {
            get { return _menuTextAlign; }
            set { _menuTextAlign = value; Initialize(_menuPosition, _menuTextAlign); }
        }
        public Fonts Font
        {
            get { return _menuFont; }
            set { _menuFont = value; }
        }
        public SpriteFont ItemsFont => Game.FontManager[Font];
        public int ItemsCount => _items.Count;
        public Menu(params MenuItem[] items)
        {
            _menuPosition = Position.Center;
            _menuTextAlign = Align.Center;
            _menuFont = Fonts.RegularConsolas16;
            _items = new List<MenuItem>();
            AddItem(items);

            Initialize();
        }
        public Menu(Position position, Align textAlign, Fonts font, params MenuItem[] items)
        {
            _menuPosition = position;
            _menuTextAlign = textAlign;
            _menuFont = font;
            _items = new List<MenuItem>();
            AddItem(items);

            Initialize(position, textAlign, font);
        }
        public void Initialize(Position position = Position.Center, Align textAlign = Align.Center, Fonts font = Fonts.RegularConsolas16)
        {
            _center = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            _itemHeight = ItemsFont.MeasureString(_items[0].Text).Y;

            _maxItemWidth = 0;

            foreach (var item in _items)
            {
                var width = ItemsFont.MeasureString(item.Text).X;
                item.Size = new Point((int)width, (int)_itemHeight);
                if (width > _maxItemWidth) _maxItemWidth = width;
            }

            _maxItemWidth *= 1.2f;

            var pos = Vector2.Zero;
            switch (position)
            {
                case Position.Top: { pos = new Vector2(_center.X, _itemHeight / 2); break; }
                case Position.Bottom: { pos = new Vector2(_center.X, Game.Window.ClientBounds.Height - _items.Count * _itemHeight + _itemHeight / 2); break; }
                case Position.Right: { pos = new Vector2(Game.Window.ClientBounds.Width - _maxItemWidth / 2, _center.Y - _items.Count / 2 * _itemHeight + _itemHeight / 2.5f); break; }
                case Position.Left: { pos = new Vector2(_maxItemWidth / 2, _center.Y - _items.Count / 2 * _itemHeight + _itemHeight / 2.5f); break; }
                case Position.Center: { pos = new Vector2(_center.X, _center.Y - _items.Count / 2 * _itemHeight + _itemHeight / 2.5f); break; }
                case Position.TopLeft: { pos = new Vector2(_maxItemWidth / 2, _itemHeight / 2); break; }
                case Position.TopRight: { pos = new Vector2(Game.Window.ClientBounds.Width - _maxItemWidth / 2, _itemHeight / 2); break; }
                case Position.BottomLeft: { pos = new Vector2(_maxItemWidth / 2, Game.Window.ClientBounds.Height - _items.Count * _itemHeight + _itemHeight / 2); break; }
                case Position.BottomRight: { pos = new Vector2(Game.Window.ClientBounds.Width - _maxItemWidth / 2, Game.Window.ClientBounds.Height - _items.Count * _itemHeight + _itemHeight / 2); break; }
            }

            foreach (var item in _items)
            {
                item.Rect = new Rectangle(
                    (int)(pos.X - _maxItemWidth / 2),
                    (int)(pos.Y - _itemHeight / 2),
                    (int)_maxItemWidth,
                    (int)_itemHeight
                );

                pos.Y += _itemHeight;
            }

            _selectedItemRect = new Texture2D(Game.Graphics.GraphicsDevice, 1, 1);
            _selectedItemRect.SetData(new[] { Color.White });
        }
        public void Invoke() => SelectedItem.Action.Invoke();
        public void AddItem(params MenuItem[] items)
        {
            foreach (var item in items) _items.Add(item);
        }
        public void Update(GameTime gameTime)
        {
            _mousePosition = GMouse.GetState().Position;

            _itemSelected = false;

            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Rect.Contains(_mousePosition))
                {
                    SelectedItemIndex = i;
                    _itemSelected = true;
                }
            }

            if (GKeyboard.IsKeyPressed(Keys.Up))
            {
                SelectedItemIndex -= 1;
            }
            if (GKeyboard.IsKeyPressed(Keys.Down))
            {
                SelectedItemIndex += 1;
            }
            if (SelectedItem.Action != null && SelectedItem.Type == ActionType.Update && (GKeyboard.IsKeyPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
        public void Draw(GameTime gameTime)
        {
            var position = new Vector2(_items.First().Rect.X + _items.First().Rect.Width / 2, _items.First().Rect.Y + _items.First().Rect.Height / 6);

            Game.SpriteBatch.Begin();
            //_spriteBatch.DrawString(_font, _items[0].Text, center - _font.MeasureString(_items[0].Text) / 2, Color.Red);

            if (SelectedItemIndex != -1)
            {
                var item = _items[SelectedItemIndex];
                Game.SpriteBatch.Draw(_selectedItemRect, item.Rect, _selectedItemRectColor);
            }

            foreach (var item in _items)
            {
                if (_menuTextAlign == Align.Center) Game.SpriteBatch.DrawString(ItemsFont, item.Text, new Vector2(position.X - ItemsFont.MeasureString(item.Text).X / 2, position.Y), Color.Black);
                if (_menuTextAlign == Align.Left) Game.SpriteBatch.DrawString(ItemsFont, item.Text, new Vector2(position.X - _maxItemWidth / 2, position.Y), Color.Black);
                if (_menuTextAlign == Align.Right) Game.SpriteBatch.DrawString(ItemsFont, item.Text, new Vector2(position.X + _maxItemWidth / 2 - ItemsFont.MeasureString(item.Text).X, position.Y), Color.Black);
                position.Y += _itemHeight;
            }

            Game.SpriteBatch.End();

            if (SelectedItem.Action != null && SelectedItem.Type == ActionType.Draw && (GKeyboard.IsKeyPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
    }
}
