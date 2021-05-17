using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GennadichGame
{
    public enum AngleNormalizationFactor
    {
        PositiveOnly,
        AllowNegative
    }
    public class GDartsSegment
    {
        public double NearDistance { get; }
        public double FarDistance { get; }
        public float FirstAngle { get; }
        public float SecondAngle { get; }
        public GDartsSegment(double nearDistance, double farDistance, float firstAngle, float secondAngle)
        {
            if (nearDistance < 0 || nearDistance > 1 || farDistance < 0 || farDistance > 1)
                throw new ArgumentOutOfRangeException("Distance must be in [0; 1]");

            NearDistance = nearDistance;
            FarDistance = farDistance;
            FirstAngle = NormalizeAngle(firstAngle, AngleNormalizationFactor.AllowNegative);
            SecondAngle = NormalizeAngle(secondAngle, AngleNormalizationFactor.AllowNegative);
        }
        public override string ToString()
        {
            return $"({NearDistance}; {FarDistance}), ({FirstAngle}; {SecondAngle})";
        }
        public static implicit operator GDartsSegment((double nearDistance, double farDistance, float firstAngle, float secondAngle) segment)
        {
            return new GDartsSegment(segment.nearDistance, segment.farDistance, segment.firstAngle, segment.secondAngle);
        }
        public static float NormalizeAngle(float angle, AngleNormalizationFactor factor = AngleNormalizationFactor.PositiveOnly)
        {
            switch (factor)
            {
                case AngleNormalizationFactor.AllowNegative:
                {
                    if (!(angle >= -360 && angle <= 360))
                    {
                        while (angle > 360) angle -= 360;
                        while (angle < -360) angle += 360;
                    }
                    return angle;
                }
                default:
                {
                    if (!(angle >= 0 && angle <= 360))
                    {
                        while (angle > 360) angle -= 360;
                        while (angle < 0) angle += 360;
                    }
                    return angle;
                }
            }
        }
    }
    public class GDarts
    {
        private GennadichGame _game;
        private Texture2D _dartsTex;
        private float _dartsScale;
        private Vector2 _dartsPosition;
        private List<GDartsSegment> _segments;
        public Vector2 DartsCenter => _dartsPosition + new Vector2(_dartsTex.Width / 2 * _dartsScale, _dartsTex.Height / 2 * _dartsScale);
        public Vector2 DartsSize => new Vector2(_dartsTex.Width * _dartsScale, _dartsTex.Height * _dartsScale);
        public GDartsSegment IntersectedSegment => _segments.FirstOrDefault(segment => Intersects(segment));
        public GDarts(GennadichGame game, Texture2D dartsTex)
        {
            _game = game;
            _dartsTex = dartsTex;

            _dartsScale = 0.5f;
            _dartsPosition = new Vector2(
                _game.Window.ClientBounds.Width / 2 - _dartsTex.Width * _dartsScale / 2 + _game.Window.ClientBounds.Width * 0.2f,
                _game.Window.ClientBounds.Height / 2 - _dartsTex.Height * _dartsScale / 2
            );


            var segmentAngle = 360f / 20;
            _segments = new List<GDartsSegment>();

            for (var angle = 0f; angle < 360; angle += segmentAngle)
            {
                _segments.Add((0, 1, angle, angle + segmentAngle));
            }

        }
        public void Update()
        {
            _game.SetCursor(_game.ArrowCursorTex);
        }
        public void Draw()
        {
            _game.SpriteBatch.Begin();
            
            _game.SpriteBatch.Draw(_dartsTex, new Rectangle(_dartsPosition.ToPoint(), DartsSize.ToPoint()), Color.White);

            var mousePosition = GetMousePositionParams();
            _game.SpriteBatch.DrawString(_game.SpriteFont, $"Distance: {(mousePosition.Distance > 1 ? 1 : mousePosition.Distance)}", new Vector2(50, 50), Color.Black);
            _game.SpriteBatch.DrawString(_game.SpriteFont, $"Angle: {GDartsSegment.NormalizeAngle(mousePosition.Angle)}", new Vector2(50, 50 + _game.SpriteFont.MeasureString("TEST").Y), Color.Black);
            _game.SpriteBatch.DrawString(_game.SpriteFont, $"Intersected segment: {IntersectedSegment}", new Vector2(50, 50 + _game.SpriteFont.MeasureString("TEST").Y * 2), Color.Black);

            _game.SpriteBatch.End();
        }
        private bool Intersects(GDartsSegment segment)
        {
            var position = GetMousePositionParams();

            if (segment.FirstAngle >= 0 && segment.SecondAngle >= 0 && position.Angle < 0)
            {
                position.Angle = GDartsSegment.NormalizeAngle(position.Angle, AngleNormalizationFactor.PositiveOnly);
            }

            if (segment.FirstAngle < segment.SecondAngle)
            {
                return position.Distance > segment.NearDistance &&
                       position.Distance < segment.FarDistance &&
                       position.Angle > segment.FirstAngle &&
                       position.Angle < segment.SecondAngle;
            }
            else
            {
                return position.Distance > segment.NearDistance &&
                       position.Distance < segment.FarDistance &&
                       position.Angle - 360 > segment.FirstAngle &&
                       position.Angle < segment.SecondAngle;
            }
        }
        private (float Distance, float Angle) GetMousePositionParams()
        {
            var mouseState = Mouse.GetState();
            var distance = (Vector2.Distance(DartsCenter, mouseState.Position.ToVector2()) / ((DartsSize.X + DartsSize.Y) / 2) * 2);
            var mouseVector = mouseState.Position.ToVector2();
            var angle = (float)(-Math.Atan2(mouseVector.Y - DartsCenter.Y, mouseVector.X - DartsCenter.X) / Math.PI * 180);

            return (distance, GDartsSegment.NormalizeAngle(angle, AngleNormalizationFactor.AllowNegative));
        }
    }
}
