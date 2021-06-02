using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Scenes
{
    public delegate void ActivateEventHandler(Scene scene);
    public delegate void DeactivateEventHandler(Scene scene);
    public abstract class Scene
    {
        private bool _active = false;
        private static GennadichGame _game;
        public bool Active => _active;
        public static GennadichGame Game => _game;
        public event ActivateEventHandler OnActivate;
        public event DeactivateEventHandler OnDeactivate;
        public void Activate()
        {
            _active = true;
            if (OnActivate != null) OnActivate.Invoke(this);
        }
        public void Deactivate()
        {
            _active = false;
            if (OnDeactivate != null) OnDeactivate.Invoke(this);
        }
        protected abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
        public static void Initialize(GennadichGame game)
        {
            if (game == null) throw new ArgumentNullException($"{nameof(game)} must not be null reference!");
            _game = game;
        }
    }
}
