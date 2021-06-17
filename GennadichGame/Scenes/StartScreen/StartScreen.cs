using System.Timers;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Scenes.StartScreen
{
    public sealed class StartScreen : Scene
    {
        private Timer _timer;
        public StartScreen()
        {
            Initialize();
        }
        public override void Initialize()
        {
            _timer = new Timer(2000);
            _timer.Elapsed += (s, e) =>
            {
                _timer.Enabled = false;
                Game.SceneManager.ActiveState = GameState.MainMenu;
            };
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            var tex = Game.TextureManager[Textures.Logo];
            var scale = .15f;
            Game.SpriteBatch.Draw(tex, new Rectangle((int)(Game.Center.X - tex.Width * scale / 2), (int)(Game.Center.Y - tex.Height * scale / 2), (int)(tex.Width * scale), (int)(tex.Height * scale)), Color.White);
            Game.SpriteBatch.End();

            if (!_timer.Enabled) _timer.Enabled = true;
        }
    }
}
