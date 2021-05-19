using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;

namespace GennadichGame.Scenes.Darts
{
    public class GDarts : Scene
    {
        public bool Active { get; set; } = false;
        public event ActivateHandler OnActivate;
        public event DeactivateHandler OnDeactivate;
        private GennadichGame _game;
        private Texture2D _dartsTex;
        private float _dartsScale;
        private Vector2 _dartsPosition;
        private List<GDartsSegment> _segments;
        public Vector2 DartsCenter => _dartsPosition + DartsSize / 2;
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

            OnActivate = () =>
            {
                _game.CurrentBackground = BackgroundImage.Clouds;
                _game.CurrentCursor = Cursor.Dart;
            };

            OnDeactivate = () => { };

            var segmentAngle = 360f / 20;
            _segments = new List<GDartsSegment>();

            _segments.Add((0, 0.02, 0, 360));
            _segments.Add((0.02, 0.04, 0, 360));
            _segments.Add((0.04, 0.07, 0, 360));
            _segments.Add((0.78, 1, 0, 360));

            for (var angle = 0f; angle < 360; angle += segmentAngle)
            {
                _segments.Add((0.07, 0.46, angle, angle + segmentAngle));
                _segments.Add((0.46, 0.5, angle, angle + segmentAngle));
                _segments.Add((0.5, 0.73, angle, angle + segmentAngle));
                _segments.Add((0.73, 0.78, angle, angle + segmentAngle));
            }
        }
        public void Activate() => OnActivate.Invoke();
        public void Deactivate() => OnDeactivate.Invoke();
        public void Update(GameTime gameTime)
        {
            
        }
        public void Draw(GameTime gameTime)
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
