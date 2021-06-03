using System;

using Microsoft.Xna.Framework;

namespace GennadichGame.Scenes.Lobby
{
    class LobbyItem
    {
        public String Text { get; set; }
        public Point Size { get; set; }
        public Rectangle Rect { get; set; }
        public LobbyItem(String text)
        {
            this.Text = text;
        }
        public static implicit operator LobbyItem(String text)
        {
            return new LobbyItem(text);
        }
    }
}
