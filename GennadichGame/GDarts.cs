using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GennadichGame
{
    public enum AngleNormalizeFactor
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
        public GDartsSegment((double near, double far) distance, (float first, float second) angle)
        {
            if (distance.near < 0 || distance.near > 1 || distance.far < 0 || distance.far > 1)
                throw new ArgumentOutOfRangeException("Distance must be in [0; 1]");

            NearDistance = distance.near;
            FarDistance = distance.far;
            FirstAngle = NormalizeAngle(angle.first, AngleNormalizeFactor.AllowNegative);
            SecondAngle = NormalizeAngle(angle.second, AngleNormalizeFactor.AllowNegative);
        }
        public static float NormalizeAngle(float angle, AngleNormalizeFactor factor = AngleNormalizeFactor.PositiveOnly)
        {
            switch (factor)
            {
                case AngleNormalizeFactor.AllowNegative:
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
        private GDartsSegment _testSegment = new GDartsSegment((0.4, 0.8), (-45, 45));
        public Vector2 DartsCenter => _dartsPosition + new Vector2(_dartsTex.Width / 2 * _dartsScale, _dartsTex.Height / 2 * _dartsScale);
        public Vector2 DartsSize => new Vector2(_dartsTex.Width * _dartsScale, _dartsTex.Height * _dartsScale);
        public GDartsSegment IntersectedSegment => _segments.FirstOrDefault(segment => Intersect(segment));
        public GDarts(GennadichGame game, Texture2D dartsTex)
        {
            _game = game;
            _dartsTex = dartsTex;

            _dartsScale = 0.5f;
            _dartsPosition = new Vector2(
                _game.Window.ClientBounds.Width / 2 - _dartsTex.Width * _dartsScale / 2 + _game.Window.ClientBounds.Width * 0.2f,
                _game.Window.ClientBounds.Height / 2 - _dartsTex.Height * _dartsScale / 2
            );

            _segments = new List<GDartsSegment>()
            {
                
            };
        }
        public void Update()
        {

        }
        public void Draw()
        {
            _game.SpriteBatch.Begin();
            
            _game.SpriteBatch.Draw(_dartsTex, new Rectangle(_dartsPosition.ToPoint(), DartsSize.ToPoint()), Color.White);

            var mousePosition = GetMousePositionParams();
            _game.SpriteBatch.DrawString(_game.SpriteFont, $"Distance from center: {(mousePosition.Distance > 1 ? 1 : mousePosition.Distance)}", new Vector2(50, 50), Color.Black);
            _game.SpriteBatch.DrawString(_game.SpriteFont, $"Angle: {(mousePosition.Angle < 0 ? mousePosition.Angle + 360 : mousePosition.Angle)}", new Vector2(50, 50 + _game.SpriteFont.MeasureString("TEST").Y), Color.Black);
            _game.SpriteBatch.DrawString(_game.SpriteFont, $"Intersects segment: {Intersect(_testSegment)}", new Vector2(50, 50 + _game.SpriteFont.MeasureString("TEST").Y * 2), Color.Black);

            _game.SpriteBatch.End();
        }
        private bool Intersect(GDartsSegment segment)
        {
            var position = GetMousePositionParams();
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

            return false;
        }
        private (float Distance, float Angle) GetMousePositionParams()
        {
            var mouseState = Mouse.GetState();
            var distance = (Vector2.Distance(DartsCenter, mouseState.Position.ToVector2()) / ((DartsSize.X + DartsSize.Y) / 2) * 2);
            var mouseVector = mouseState.Position.ToVector2();
            var angle = (float)(-Math.Atan2(mouseVector.Y - DartsCenter.Y, mouseVector.X - DartsCenter.X) / Math.PI * 180);

            return (distance, GDartsSegment.NormalizeAngle(angle, AngleNormalizeFactor.AllowNegative));
        }
    }
}
