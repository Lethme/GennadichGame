using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GennadichGame.Enums;
using GennadichGame.Input;

namespace GennadichGame.Scenes.Darts
{
    public sealed class DartsSegmentScore
    {
        public Vector2 AngleRange;
        public int Score;

        public DartsSegmentScore(int startAngle, int endAngle, int score)
        {
            AngleRange = new Vector2(startAngle, endAngle);
            Score = score;
        }
    }

    public sealed class Darts
    {
        private Texture2D _dartsTex;
        private List<GDartsSegment> _segments;
        public List<DartsSegmentScore> SegmentScores;
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

            SegmentScores = new List<DartsSegmentScore>
            {
                new DartsSegmentScore(0, 18, 13),
                new DartsSegmentScore(18, 36, 4),
                new DartsSegmentScore(36, 54, 18),
                new DartsSegmentScore(54, 72, 1),
                new DartsSegmentScore(72, 90, 20),
                new DartsSegmentScore(90, 108, 5),
                new DartsSegmentScore(108, 126, 12),
                new DartsSegmentScore(126, 144, 9),
                new DartsSegmentScore(144, 162, 14),
                new DartsSegmentScore(162, 180, 11),
                new DartsSegmentScore(180, 198, 8),
                new DartsSegmentScore(198, 216, 16),
                new DartsSegmentScore(216, 234, 7),
                new DartsSegmentScore(234, 252, 19),
                new DartsSegmentScore(252, 270, 3),
                new DartsSegmentScore(270, 288, 17),
                new DartsSegmentScore(288, 306, 2),
                new DartsSegmentScore(306, 324, 15),
                new DartsSegmentScore(324, 342, 10),
                new DartsSegmentScore(342, 360, 6),
            };
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
            var angle = (float) (-Math.Atan2(mouseVector.Y - Center.Y, mouseVector.X - Center.X) / Math.PI * 180);

            return (distance, GDartsSegment.NormalizeAngle(angle, AngleNormalizationFactor.AllowNegative));
        }

        public int GetIntersectedSegmentScore()
        {
            var segment = IntersectedSegment;
            var multiplicationFactor = 1;
            if (segment.NearDistance == 0.78)
            {
                return 0;
            }
            
            if (segment.FarDistance == 0.02)
            {
                return 50;
            }

            if (segment.NearDistance == 0.02 && segment.FarDistance == 0.07)
            {
                return 25;
            }

            if (segment.NearDistance == 0.46 && segment.FarDistance == 0.5)
            {
                multiplicationFactor = 3;
            }
            if (segment.NearDistance == 0.73 && segment.FarDistance == 0.78)
            {
                multiplicationFactor = 2;
            }

            foreach (var segmentScore in SegmentScores)
            {
                if (segment.FirstAngle == segmentScore.AngleRange.X &&
                    segment.SecondAngle == segmentScore.AngleRange.Y)
                    return multiplicationFactor * segmentScore.Score;
            }

            return 0;
        }
    }
}