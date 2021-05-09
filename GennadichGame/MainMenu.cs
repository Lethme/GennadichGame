using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GennadichGame
{
    public enum ActionType
    {
        Update,
        Draw
    }
    public class MainMenuItem
    {
        public String Text { get; set; }
        public Point Size { get; set; }
        public Rectangle Rect { get; set; }
        public Action Action { get; set; }
        public ActionType Type { get; }
        public MainMenuItem(String text, ActionType actionType, Action action)
        {
            this.Text = text;
            this.Action = action;
            this.Type = actionType;
        }
    }
    public class MainMenu
    {
        private GennadichGame _game;
        private SpriteFont _font;
        private List<MainMenuItem> _items;
        private Point _mousePosition;
        private Texture2D _selectedItemRect;
        private Texture2D _arrowCursorTex;
        private Texture2D _pointerCursorTex;
        private Color _selectedItemRectColor = Color.Aquamarine;
        private int _selectedItemIndex = 0;
        private bool _itemSelected;
        private Vector2 _center;
        private float _itemHeight;
        private float _maxItemWidth;
        private MainMenuItem SelectedItem => _items[_selectedItemIndex];
        private int SelectedItemIndex 
        { 
            get { return _selectedItemIndex; }
            set { if (!(value < 0 || value > ItemsCount - 1)) _selectedItemIndex = value; }
        }
        public int ItemsCount => _items.Count;
        public MainMenu(GennadichGame game, SpriteFont spriteFont, params MainMenuItem[] items)
        {
            _items = new List<MainMenuItem>();
            _game = game;
            _font = spriteFont;

            _arrowCursorTex = _game.ArrowCursorTex;
            _pointerCursorTex = _game.PointerCursorTex;

            AddItem(items);

            _center = new Vector2(_game.Window.ClientBounds.Width / 2, _game.Window.ClientBounds.Height / 2);
            _itemHeight = _font.MeasureString(_items[0].Text).Y;

            _game.Background = _game.Backgrounds["clouds"];

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

            _selectedItemRect = new Texture2D(_game.Graphics.GraphicsDevice, 1, 1);
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

            _itemSelected = false;

            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Rect.Contains(_mousePosition))
                {
                    Mouse.SetCursor(MouseCursor.FromTexture2D(_pointerCursorTex, 0, 0));
                    SelectedItemIndex = i;
                    _itemSelected = true;
                }
            }

            if (!_itemSelected) Mouse.SetCursor(MouseCursor.FromTexture2D(_arrowCursorTex, 0, 0));

            if (Keyboard.HasBeenPressed(Keys.Up))
            {
                SelectedItemIndex -= 1;
            }
            if (Keyboard.HasBeenPressed(Keys.Down))
            {
                SelectedItemIndex += 1;
            }
            if (SelectedItem.Type == ActionType.Update && (Keyboard.HasBeenPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
        public void Draw()
        {
            var position = new Vector2(_center.X, _center.Y - _items.Count / 2 * _itemHeight);

            _game.SpriteBatch.Begin();
            //_spriteBatch.DrawString(_font, _items[0].Text, center - _font.MeasureString(_items[0].Text) / 2, Color.Red);

            if (SelectedItemIndex != -1)
            {
                var item = _items[SelectedItemIndex];
                _game.SpriteBatch.Draw(_selectedItemRect, item.Rect, _selectedItemRectColor);
            }

            foreach (var item in _items)
            {
                _game.SpriteBatch.DrawString(_font, item.Text, new Vector2(position.X - _font.MeasureString(item.Text).X / 2, position.Y), Color.Black);
                position.Y += _itemHeight;
            }

            _game.SpriteBatch.End();

            if (SelectedItem.Type == ActionType.Draw && (Keyboard.HasBeenPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
    }
}
