using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;

namespace GennadichGame.Manager
{
    public class CursorManager
    {
        private Cursor _currentCursor;
        private Texture2D _currentCursorTex;
        public Cursor ActiveCursor
        {
            get { return _currentCursor; }
            set { SetActiveCursor(value); }
        }
        private Dictionary<Cursor, Texture2D> Cursors { get; } = new Dictionary<Cursor, Texture2D>();
        public CursorManager() { }
        public CursorManager(params KeyValuePair<Cursor, Texture2D>[] cursors) => AddCursor(cursors);
        public CursorManager(params (Cursor state, Texture2D cursorTexture)[] cursors) => AddCursor(cursors);
        public void AddCursor(params KeyValuePair<Cursor, Texture2D>[] cursors)
        {
            foreach (var cursor in cursors) Cursors.Add(cursor.Key, cursor.Value);
        }
        public void AddCursor(params (Cursor state, Texture2D cursorTexture)[] cursors)
        {
            foreach (var cursor in cursors) Cursors.Add(cursor.state, cursor.cursorTexture);
        }
        public void SetActiveCursor(Cursor cursor)
        {
            if (SetActiveCursor(Cursors[cursor])) _currentCursor = cursor;
        }
        private bool SetActiveCursor(Texture2D cursorTexture)
        {
            if (_currentCursorTex != cursorTexture)
            {
                Mouse.SetCursor(MouseCursor.FromTexture2D(cursorTexture, 0, 0));
                _currentCursorTex = cursorTexture;
                return true;
            }

            return false;
        }
    }
}
