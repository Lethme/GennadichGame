using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;
using GennadichGame.Input;

namespace GennadichGame.Controls.Input
{
    public class InputBox : GameModule
    {
        private static Texture2D _bgTexture;
        private static bool _show = false;
        public static Fonts? Font { get; set; } = Fonts.RegularConsolas14;
        public static Color? FontColor { get; set; } = Color.Black;
        public static String Text { get; set; } = String.Empty;
        public static float Width { get; set; } = .5f;
        public static Point Size => new Point((int)(Game.Size.X * Width), (int)(Game.FontManager[Font.Value].MeasureString("A").Y));
        public static void Initialize()
        {
            _bgTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            _bgTexture.SetData(new [] { Color.White });
        }
        public static void Show()
        {
            if (!_show) Text = String.Empty;
            _show = true;
            Update();
            Draw();
        }
        public static void Update()
        {
            if (GKeyboard.IsKeyPressed(Keys.Enter))
            {
                _show = false;
            }
        }
        public static void Draw()
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(_bgTexture, new Rectangle((int)(Game.Center.X - Size.X / 2), (int)(Game.Center.Y - Size.Y / 2), Size.X, (int)(Size.Y * 1.2f)), Color.Orange);
            Game.SpriteBatch.End();
        }
    }
}
