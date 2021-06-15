using System;

using Microsoft.Xna.Framework;

using GennadichGame.Controls;

namespace GennadichGame.Scenes
{
    public delegate void ActivateEventHandler(Scene scene);
    public delegate void DeactivateEventHandler(Scene scene);
    public abstract class Scene : GameModule
    {
        private bool _active = false;
        public bool Active => _active;
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

        public virtual void Initialize() { }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
