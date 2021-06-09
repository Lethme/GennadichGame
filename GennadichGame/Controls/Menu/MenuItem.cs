using System;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Controls
{
    public class MenuItem
    {
        public String Text { get; set; }
        public Point Size { get; set; }
        public Rectangle Rect { get; set; }
        public Action Action { get; set; }
        public ActionType Type { get; }
        public MenuItem(String text)
        {
            this.Text = text;
        }
        public MenuItem(String text, ActionType actionType, Action action)
        {
            this.Text = text;
            this.Action = action;
            this.Type = actionType;
        }
        public static implicit operator MenuItem(String item)
        {
            return new MenuItem(item);
        }
        public static implicit operator MenuItem((String text, ActionType actionType, Action action) item)
        {
            return new MenuItem(item.text, item.actionType, item.action);
        }
    }
}
