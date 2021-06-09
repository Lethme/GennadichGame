using System;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Controls
{
    public abstract class Control : GameModule
    {
        protected static String DefaultText = String.Empty;
        protected static Point DefaultLocation = Point.Zero;
        protected static Fonts DefaultFont = Fonts.RegularConsolas14;
        protected static Color DefaultFontColor = Color.Black;
        protected static Position DefaultPosition = Position.TopLeft;
        protected static Align DefaultTextAlign = Align.Left;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
