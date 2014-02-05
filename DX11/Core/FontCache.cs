using System;
using System.Collections.Generic;
using SlimDX;
using SlimDX.DirectWrite;
using SpriteTextRenderer;

namespace Core {
    public class FontCache : DisposableClass {
        private bool _disposed;

        private readonly Dictionary<string, TextBlockRenderer> _fonts = new Dictionary<string, TextBlockRenderer>();
        private readonly SpriteRenderer _sprite;
        private TextBlockRenderer _default;

        public FontCache(SpriteRenderer sprite) {
            _sprite = sprite;
            _default = new TextBlockRenderer(sprite, "Arial", FontWeight.Normal, FontStyle.Normal, FontStretch.Normal, 12);
        }

        protected override void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    Util.ReleaseCom(ref _default);
                    foreach (var textBlockRenderer in _fonts) {
                        var f = textBlockRenderer.Value;
                        Util.ReleaseCom(ref f);
                    }
                    _fonts.Clear();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        public bool RegisterFont(string name, float fontSize, string fontFace = "Arial", FontWeight fontWeight = FontWeight.Normal, FontStyle fontStyle = FontStyle.Normal, FontStretch fontStretch = FontStretch.Normal) {
            if (_fonts.ContainsKey(name)) {
                Console.WriteLine("Duplicate font name: " + name);
                return false;
            }
            try {
                _fonts[name] = new TextBlockRenderer(_sprite, fontFace, fontWeight, fontStyle, fontStretch, fontSize);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public StringMetrics DrawString(string fontName, string text, Vector2 location, Color4 color) {
            if (!_fonts.ContainsKey(fontName)) {
                return _default.DrawString(text, location, color);
            }
            return _fonts[fontName].DrawString(text, location, color);
        }
        public StringMetrics DrawString(string text, Vector2 location, Color4 color) {

            return _default.DrawString(text, location, color);
        }
    }
}