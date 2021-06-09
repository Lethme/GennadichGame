using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;
using GennadichGame.Controls;

namespace GennadichGame.Manager
{
    public class TextureManager : GameModule, IEnumerable<KeyValuePair<Textures, Texture2D>>
    {
        private Dictionary<Textures, Texture2D> Textures { get; } = new Dictionary<Textures, Texture2D>();
        public TextureManager() { }
        public TextureManager(params KeyValuePair<Textures, Texture2D>[] textures) => AddTexture(textures);
        public TextureManager(params (Textures textureID, Texture2D texture)[] textures) => AddTexture(textures);
        public Texture2D this[Textures texture] => Textures[texture];
        public void AddTexture(params KeyValuePair<Textures, Texture2D>[] textures)
        {
            foreach (var texture in textures) if (!Textures.ContainsKey(texture.Key)) Textures.Add(texture.Key, texture.Value);
        }
        public void AddTexture(params (Textures textureID, Texture2D texture)[] textures)
        {
            foreach (var texture in textures) if (!Textures.ContainsKey(texture.textureID)) Textures.Add(texture.textureID, texture.texture);
        }
        public Texture2D GetTexture(Textures texture) => Textures[texture];
        public bool Contains(Textures texture) => Textures.ContainsKey(texture);
        public bool Release(Textures texture)
        {
            if (Textures.ContainsKey(texture))
            {
                var tex = Textures[texture];
                Textures.Remove(texture);
                tex.Dispose();
                return true;
            }

            return false;
        }
        public IEnumerator<KeyValuePair<Textures, Texture2D>> GetEnumerator()
        {
            return Textures.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Textures.GetEnumerator();
        }
    }
}
