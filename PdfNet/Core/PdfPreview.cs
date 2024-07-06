using System;
using System.Numerics;

namespace PdfNet.Core
{
    public class PdfPreview
    {
        private PdfViewport _viewport;
        private PdfDocument _document;
        private PdfTexture _texture;
    
        public Vector2 Resolution { get; }
    
        public PdfPreview(PdfDocument document, float scale)
        {
            _document = document;
            Resolution = _document.DocumentSize * scale;
            _viewport = new PdfViewport(Resolution);
            _texture = new PdfTexture(Resolution);
        }

        public PdfTexture Render()
        {
            for(var i = 0; i < _document.PageCount; i++)
            {
                var page = _document.GetPage(i);
                page.UpdatePageSize(_viewport);
                page.Render(_viewport, _texture, 1.0f);
            }

            return _texture;
        }
    }
}