using System;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Controls
{
    public class Label : Control
    {
        public Point TextSize => Game.FontManager[Font].MeasureString(Text).ToPoint();
        public String Text { get; set; } = DefaultText;
        public Point Location { get; set; } = DefaultLocation;
        public Fonts Font { get; set; } = DefaultFont;
        public Color FontColor { get; set; } = DefaultFontColor;
        public Position Position { set { SetPosition(value); } }
        public Label() { }
        public Label(String text = null, Position? position = null, Fonts? font = null, Color? fontColor = null)
        {
            if (text != null) Text = text;
            if (font != null) Font = font.Value;
            if (fontColor != null) FontColor = fontColor.Value;
            if (position != null) Position = position.Value;
        }
        public Label(String text = null, Point? location = null, Fonts? font = null, Color? fontColor = null)
        {
            if (text != null) Text = text;
            if (font != null) Font = font.Value;
            if (fontColor != null) FontColor = fontColor.Value;
            if (location != null) Location = location.Value;
        }
        public void SetPosition(Position position)
        {
            switch (position)
            {
                case Position.Top: { Location = new Point(Game.Center.X - TextSize.X / 2, 0); break; }
                case Position.Bottom: { Location = new Point(Game.Center.X - TextSize.X / 2, Game.Size.Y - (int)(TextSize.Y / 1.5f));  break; }
                case Position.Left: { Location = new Point(0, Game.Center.Y - TextSize.Y / 2); break; }
                case Position.Right: { Location = new Point(Game.Size.X - TextSize.X, Game.Center.Y - TextSize.Y / 2); break; }
                case Position.TopLeft: { Location = Point.Zero; break; }
                case Position.TopRight: { Location = new Point(Game.Size.X - TextSize.X, 0); break; }
                case Position.BottomLeft: { Location = new Point(0, Game.Size.Y - (int)(TextSize.Y / 1.5f)); break; }
                case Position.BottomRight: { Location = new Point(Game.Size.X - TextSize.X, Game.Size.Y - (int)(TextSize.Y / 1.5f)); break; }
                case Position.Center: { Location = new Point(Game.Center.X - TextSize.X / 2, Game.Center.Y - TextSize.Y / 2); break; }
            }
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.DrawString(Game.FontManager[Font], Text, Location.ToVector2(), FontColor);
            Game.SpriteBatch.End();
        }
    }
}
