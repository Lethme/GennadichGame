using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;

namespace GennadichGame.Manager
{
    public class TextureManager
    {
        private Dictionary<Textures, Texture2D> Textures { get; } = new Dictionary<Textures, Texture2D>();
        public TextureManager() { }
        public TextureManager(params KeyValuePair<Textures, Texture2D>[] textures) => AddTexture(textures);
        public TextureManager(GennadichGame game, params (Textures textureID, Texture2D texture)[] textures) => AddTexture(textures);
        public Texture2D this[Textures texture] => Textures[texture];
        public void AddTexture(params KeyValuePair<Textures, Texture2D>[] textures)
        {
            foreach (var texture in textures) Textures.Add(texture.Key, texture.Value);
        }
        public void AddTexture(params (Textures textureID, Texture2D texture)[] textures)
        {
            foreach (var texture in textures) Textures.Add(texture.textureID, texture.texture);
        }
        public Texture2D GetTexture(Textures texture) => Textures[texture];
    }
}
