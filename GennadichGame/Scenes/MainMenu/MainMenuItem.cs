using System;
using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Scenes.Menu
{
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
}
