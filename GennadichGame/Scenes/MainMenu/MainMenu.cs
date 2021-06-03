using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Enums;
using GennadichGame.Input;

namespace GennadichGame.Scenes.Menu
{
    public sealed class MainMenu : Scene
    {
        #region Data
        private List<MainMenuItem> _items;
        private Point _mousePosition;
        private Texture2D _selectedItemRect;
        private Color _selectedItemRectColor = Color.CornflowerBlue;
        private int _selectedItemIndex = 0;
        private bool _itemSelected;
        private Vector2 _center;
        private float _itemHeight;
        private float _maxItemWidth;
        #endregion
        #region Properties
        private SpriteFont ItemsFont => Game.FontManager[Fonts.RegularConsolas18];
        private MainMenuItem SelectedItem => _items[_selectedItemIndex];
        private int SelectedItemIndex 
        { 
            get { return _selectedItemIndex; }
            set { if (!(value < 0 || value > ItemsCount - 1)) _selectedItemIndex = value; }
        }
        public int ItemsCount => _items.Count;
        #endregion
        #region Constructors
        public MainMenu(params MainMenuItem[] items)
        {
            _items = new List<MainMenuItem>();
            AddItem(items);

            Initialize();
        }
        #endregion
        #region BaseClassMethods
        protected override void Initialize()
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

            _selectedItemRect = new Texture2D(Game.Graphics.GraphicsDevice, 1, 1);
            _selectedItemRect.SetData(new[] { Color.White });
        }
        public override void Update(GameTime gameTime)
        {
            _mousePosition = Mouse.GetState().Position;

            _itemSelected = false;

            for (var i = 0; i < _items.Count; i++)
            {
                if (_items[i].Rect.Contains(_mousePosition))
                {
                    //Game.CurrentCursor = Cursor.Pointer;
                    SelectedItemIndex = i;
                    _itemSelected = true;
                }
            }

            //if (!_itemSelected) Game.CurrentCursor = Cursor.Dart;

            if (GKeyboard.IsKeyPressed(Keys.Up))
            {
                SelectedItemIndex -= 1;
            }
            if (GKeyboard.IsKeyPressed(Keys.Down))
            {
                SelectedItemIndex += 1;
            }
            if (SelectedItem.Type == ActionType.Update && (GKeyboard.IsKeyPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            var position = new Vector2(_center.X, _center.Y - _items.Count / 2 * _itemHeight);

            Game.SpriteBatch.Begin();
            //_spriteBatch.DrawString(_font, _items[0].Text, center - _font.MeasureString(_items[0].Text) / 2, Color.Red);

            if (SelectedItemIndex != -1)
            {
                var item = _items[SelectedItemIndex];
                Game.SpriteBatch.Draw(_selectedItemRect, item.Rect, _selectedItemRectColor);
            }

            foreach (var item in _items)
            {
                Game.SpriteBatch.DrawString(ItemsFont, item.Text, new Vector2(position.X - ItemsFont.MeasureString(item.Text).X / 2, position.Y), Color.Black);
                position.Y += _itemHeight;
            }

            Game.SpriteBatch.End();

            if (SelectedItem.Type == ActionType.Draw && (GKeyboard.IsKeyPressed(Keys.Enter) || (Mouse.GetState().LeftButton == ButtonState.Pressed && _itemSelected)))
            {
                Invoke();
            }
        }
        #endregion
        #region PrivateMethods
        #endregion
        #region PublicMethods
        public void Invoke() => SelectedItem.Action.Invoke();
        public void AddItem(params MainMenuItem[] items)
        {
            foreach (var item in items) _items.Add(item);
        }
        #endregion
    }
}
