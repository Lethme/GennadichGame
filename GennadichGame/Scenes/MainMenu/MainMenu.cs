using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Enums;
using GennadichGame.Input;

namespace GennadichGame.Scenes.Menu
{
    public class MainMenu : Scene
    {
        public bool Active { get; set; } = false;
        public event ActivateHandler OnActivate;
        public event DeactivateHandler OnDeactivate;
        private GennadichGame _game;
        private SpriteFont _font;
        private List<MainMenuItem> _items;
        private Point _mousePosition;
        private Texture2D _selectedItemRect;
        private Color _selectedItemRectColor = Color.CornflowerBlue;
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

            OnActivate = () =>
            {
                _game.CurrentBackground = BackgroundImage.Clouds;
                _game.CurrentCursor = Cursor.Dart;
            };

            OnDeactivate = () => { };

            AddItem(items);

            _center = new Vector2(_game.Window.ClientBounds.Width / 2, _game.Window.ClientBounds.Height / 2);
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

            _selectedItemRect = new Texture2D(_game.Graphics.GraphicsDevice, 1, 1);
            _selectedItemRect.SetData(new[] { Color.White });
        }
        public void AddItem(params MainMenuItem[] items)
        {
            foreach (var item in items) _items.Add(item);
        }
        public void Activate() => OnActivate.Invoke();
        public void Deactivate() => OnDeactivate.Invoke();
        public void Invoke() => SelectedItem.Action.Invoke();
        public void Update(GameTime gameTime)
        {
            _mousePosition = Mouse.GetState().Position;

            _itemSelected = false;

            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Rect.Contains(_mousePosition))
                {
                    //_game.CurrentCursor = Cursor.Pointer;
                    SelectedItemIndex = i;
                    _itemSelected = true;
                }
            }

            //if (!_itemSelected) _game.CurrentCursor = Cursor.Dart;

            if (GKeyboard.HasBeenPressed(Keys.Up))
            {
                SelectedItemIndex -= 1;
            }
            if (GKeyboard.HasBeenPressed(Keys.Down))
            {
                SelectedItemIndex += 1;
            }
            if (SelectedItem.Type == ActionType.Update && (GKeyboard.HasBeenPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
        public void Draw(GameTime gameTime)
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

            if (SelectedItem.Type == ActionType.Draw && (GKeyboard.HasBeenPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
    }
}
