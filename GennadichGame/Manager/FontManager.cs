using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;

namespace GennadichGame.Manager
{
    public class FontManager : IEnumerable<KeyValuePair<Fonts, SpriteFont>>
    {
        private Dictionary<Fonts, SpriteFont> Fonts { get; } = new Dictionary<Fonts, SpriteFont>();
        public FontManager() { }
        public FontManager(params KeyValuePair<Fonts, SpriteFont>[] fonts) => AddFont(fonts);
        public FontManager(params (Fonts fontID, SpriteFont font)[] fonts) => AddFont(fonts);
        public SpriteFont this[Fonts font] => GetFont(font);
        public void AddFont(params KeyValuePair<Fonts, SpriteFont>[] fonts)
        {
            foreach (var font in fonts) if (!Fonts.ContainsKey(font.Key)) Fonts.Add(font.Key, font.Value);
        }
        public void AddFont(params (Fonts fontID, SpriteFont font)[] fonts)
        {
            foreach (var font in fonts) if (!Fonts.ContainsKey(font.fontID)) Fonts.Add(font.fontID, font.font);
        }
        public SpriteFont GetFont(Fonts font) => Fonts.ContainsKey(font) ? Fonts[font] : null;
        public bool Contains(Fonts font) => Fonts.ContainsKey(font);
        public IEnumerator<KeyValuePair<Fonts, SpriteFont>> GetEnumerator()
        {
            return Fonts.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Fonts.GetEnumerator();
        }
    }
}
