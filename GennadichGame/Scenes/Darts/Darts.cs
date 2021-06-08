using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;
using GennadichGame.Input;

namespace GennadichGame.Scenes.Darts
{
    public sealed class Darts
    {
        private Texture2D _dartsTex;
        private List<GDartsSegment> _segments;
        public float Scale { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Center => Position + Size / 2;
        public Vector2 Size => new Vector2(_dartsTex.Width * Scale, _dartsTex.Height * Scale);
        public Texture2D Texture => _dartsTex;
        public GDartsSegment IntersectedSegment => _segments.FirstOrDefault(segment => Intersects(segment));
        public Darts(Texture2D dartsTex)
        {
            _dartsTex = dartsTex;

            Scale = 1f;
            Position = Vector2.Zero;

            var segmentAngle = 360f / 20;
            _segments = new List<GDartsSegment>
            {
                (0, 0.02, 0, 360),
                (0.02, 0.04, 0, 360),
                (0.04, 0.07, 0, 360),
                (0.78, 1, 0, 360)
            };

            for (var angle = 0f; angle < 360; angle += segmentAngle)
            {
                _segments.Add((0.07, 0.46, angle, angle + segmentAngle));
                _segments.Add((0.46, 0.5, angle, angle + segmentAngle));
                _segments.Add((0.5, 0.73, angle, angle + segmentAngle));
                _segments.Add((0.73, 0.78, angle, angle + segmentAngle));
            }
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
        public (float Distance, float Angle) GetMousePositionParams()
        {
            var mouseState = GMouse.GetState();
            var distance = (Vector2.Distance(Center, mouseState.Position.ToVector2()) / ((Size.X + Size.Y) / 2) * 2);
            var mouseVector = mouseState.Position.ToVector2();
            var angle = (float)(-Math.Atan2(mouseVector.Y - Center.Y, mouseVector.X - Center.X) / Math.PI * 180);

            return (distance, GDartsSegment.NormalizeAngle(angle, AngleNormalizationFactor.AllowNegative));
        }
    }
}
