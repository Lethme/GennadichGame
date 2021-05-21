using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Scenes
{
    public delegate void ActivateEventHandler();
    public delegate void DeactivateEventHandler(Scene scene);
    public abstract class Scene
    {
        private bool _active = false;
        public bool Active => _active;
        public event ActivateEventHandler OnActivate;
        public event DeactivateEventHandler OnDeactivate;
        public void Activate()
        {
            _active = true;
            if (OnActivate != null) OnActivate.Invoke();
        }
        public void Deactivate()
        {
            _active = false;
            if (OnDeactivate != null) OnDeactivate.Invoke(this);
        }
        protected abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
