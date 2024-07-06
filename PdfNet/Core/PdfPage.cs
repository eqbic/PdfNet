using System;
using System.Drawing;
using System.Numerics;
using PDFiumCore;
using PdfNet.Unsafe;

namespace PdfNet.Core
{
    public class PdfPage : IDisposable
    {
        public FpdfPageT Page { get; }


        private readonly float _aspectRatio;
        private float _startPositionY;

        private RectangleF _rectangle;
        public RectangleF Rectangle => _rectangle;
        private readonly int _index;

        public Vector2 Size => Rectangle.Size();

        public PdfPage(FpdfPageT page, int pageIndex)
        {
            Page = page;
            _index = pageIndex;
            var width = fpdfview.FPDF_GetPageWidthF(Page);
            var height = fpdfview.FPDF_GetPageHeightF(Page);
            _aspectRatio = height / width;
            _startPositionY = pageIndex * height;
            _rectangle = new RectangleF(0, _startPositionY, width, height);
        }

        public void UpdatePageSize(PdfViewport viewport)
        {
            _rectangle.Width = viewport.Size.X * viewport.Zoom;
            _rectangle.Height = _rectangle.Width * _aspectRatio;
            _rectangle.Y = _index * _rectangle.Height;
        }

        public void Render(PdfViewport viewport, PdfTexture texture, float renderScale)
        {
            UnsafeUtils.RenderPage(texture.Data, viewport.Rectangle, _rectangle, Page, viewport.Zoom, renderScale);
        }

        public void Render(PdfTexture texture)
        {
            UnsafeUtils.RenderPage(texture.Data, _rectangle, _rectangle, Page, 1f, 1f);
        }

        public void Dispose()
        {
            fpdfview.FPDF_ClosePage(Page);
        }
    }
}