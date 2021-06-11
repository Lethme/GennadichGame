using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;
using GennadichGame.Controls;

namespace GennadichGame.Controls
{
    public class MultiLabel : Control
    {
        private Position _position = DefaultPosition;
        private Align _textAlign = DefaultTextAlign;
        private Point _location = DefaultLocation;
        private List<Label> Labels { get; } = new List<Label>();
        private List<String> Texts { get; } = new List<String>();
        public Point Location { get { return _location; } set { _location = value; _position = Position.None; CalcLocations(_textAlign); } }
        public Fonts Font { get; set; } = DefaultFont;
        public Color FontColor { get; set; } = DefaultFontColor;
        public Align TextAlign { set { _textAlign = value; CalcLocations(value); } }
        public Position Position { set { _position = value; SetPosition(value); } }
        public int MaxLabelWidth => Labels.Select(label => label.TextSize.X).Max();
        public int MinLabelWidth => Labels.Select(label => label.TextSize.X).Min();
        public int MaxLabelHeight => Labels.Select(label => label.TextSize.Y).Max();
        public int MinLabelHeight => Labels.Select(label => label.TextSize.Y).Min();
        public Point Size => new Point(MaxLabelWidth, Labels.Select(label => label.TextSize.Y).Sum());
        public Point MaxLabelSize => new Point(MaxLabelWidth, MaxLabelHeight);
        public Point MinLabelSize => new Point(MinLabelWidth, MinLabelHeight);
        public Label this[int index] => Labels[index];
        public MultiLabel() { }
        public MultiLabel(Position? position = null, Align? textAlign = null, Fonts? font = null, Color? fontColor = null, params String[] captions)
        {
            if (font != null) Font = font.Value;
            if (fontColor != null) FontColor = fontColor.Value;
            if (position != null) Position = position.Value;
            if (textAlign != null) TextAlign = textAlign.Value;
            AddLabel(captions);
        }
        public MultiLabel(Point? location = null, Align? textAlign = null, Fonts? font = null, Color? fontColor = null, params String[] captions)
        {
            if (font != null) Font = font.Value;
            if (fontColor != null) FontColor = fontColor.Value;
            if (location != null) Location = location.Value;
            if (textAlign != null) TextAlign = textAlign.Value;
            AddLabel(captions);
        }
        public void AddLabel(params String[] captions)
        {
            foreach (var cap in captions) Labels.Add(new Label(cap, Location, Font, FontColor));
            foreach (var text in Labels.Select(label => label.Text)) Texts.Add(text);
            SetPosition(_position);
            CalcLocations(_textAlign);
        }
        private void CalcLocations(Align align)
        {
            if (Labels.Count == 0) return;

            var pos = Location;
            foreach (var label in Labels)
            {
                switch (align)
                {
                    case Align.Center: { pos.X += MaxLabelWidth / 2 - label.TextSize.X / 2; break; }
                    case Align.Right: { pos.X += MaxLabelWidth - label.TextSize.X; break; }
                }

                label.Location = pos;
                pos.X = Location.X;
                pos += new Point(0, label.TextSize.Y);
            }
        }
        public void SetPosition(Position position)
        {
            if (Labels.Count == 0) return;

            switch (position)
            {
                case Position.Top: { Location = new Point(Game.Center.X - MaxLabelWidth / 2, 0); break; }
                case Position.Bottom: { Location = new Point(Game.Center.X - MaxLabelWidth / 2, Game.Size.Y - Size.Y); break; }
                case Position.Left: { Location = new Point(0, Game.Center.Y - Size.Y / 2); break; }
                case Position.Right: { Location = new Point(Game.Size.X - Size.X, Game.Center.Y - Size.Y / 2); break; }
                case Position.TopLeft: { Location = Point.Zero; break; }
                case Position.TopRight: { Location = new Point(Game.Size.X - Size.X, 0); break; }
                case Position.BottomLeft: { Location = new Point(0, Game.Size.Y - Size.Y); break; }
                case Position.BottomRight: { Location = new Point(Game.Size.X - Size.X, Game.Size.Y - Size.Y); break; }
                case Position.Center: { Location = new Point(Game.Center.X - Size.X / 2, Game.Center.Y - Size.Y / 2); break; }
            }
        }
        public override void Update(GameTime gameTime)
        {
            SetPosition(_position);
            CalcLocations(_textAlign);
        }
        public override void Draw(GameTime gameTime)
        {
            foreach (var label in Labels) label.Draw(gameTime);
        }
    }
}
