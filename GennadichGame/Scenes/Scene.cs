using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using GennadichGame.Enums;

namespace GennadichGame.Scenes
{
    public delegate void ActivateEventHandler();
    public delegate void DeactivateEventHandler(Scene scene);
    public interface Scene
    {
        public bool Active { get; }
        public event ActivateEventHandler OnActivate;
        public event DeactivateEventHandler OnDeactivate;
        public void Activate();
        public void Deactivate();
        public void Update(GameTime gameTime);
        public void Draw(GameTime gameTime);
    }
}
