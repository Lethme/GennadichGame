using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace GennadichGame.Scenes
{
    public delegate void ActivateHandler(Scene scene);
    public delegate void DeactivateHandler(Scene scene);
    public interface Scene
    {
        public bool Active { get; }
        public event ActivateHandler OnActivate;
        public event DeactivateHandler OnDeactivate;
        public void Activate();
        public void Deactivate();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);
    }
}
