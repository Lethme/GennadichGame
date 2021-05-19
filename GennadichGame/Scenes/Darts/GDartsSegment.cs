using System;

using GennadichGame.Enums;

namespace GennadichGame.Scenes.Darts
{
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
}
