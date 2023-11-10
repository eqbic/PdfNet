using System;
using System.Drawing;
using PDFiumCore;
using PdfNet.Unsafe;

namespace PdfNet.Core
{
    public class PdfPage : IDisposable
    {
        public FpdfPageT Page { get; }


        private readonly float _aspectRatio;
        private int _startPositionY;

        private Rectangle _rectangle;
        public Rectangle Rectangle => _rectangle;
        private readonly int _index;

        public Vector2Int Size => new Vector2Int(Rectangle.Width, Rectangle.Height);
        public int Top => Rectangle.Top;
        public int Bottom => Rectangle.Bottom;

        public PdfPage(FpdfPageT page, int pageIndex)
        {
            Page = page;
            _index = pageIndex;
            var width = (int)fpdfview.FPDF_GetPageWidthF(Page);
            var height = (int)fpdfview.FPDF_GetPageHeightF(Page);
            _aspectRatio = (float)height / width;
            _startPositionY = pageIndex * height;
            _rectangle = new Rectangle(0, _startPositionY, width, height);
        }

        public void UpdatePageSize(PdfViewport viewport)
        {
            _rectangle.Width = (int)(viewport.Size.X * viewport.Zoom);
            _rectangle.Height = (int)(_rectangle.Width * _aspectRatio);
            _rectangle.Y = _index * _rectangle.Height;
        }

        public void Render(PdfViewport viewport, PdfTexture texture)
        {
            UnsafeUtils.RenderPage(texture.Data, viewport.Rectangle, _rectangle, Page, viewport.Zoom);
        }

        public void Dispose()
        {
            fpdfview.FPDF_ClosePage(Page);
        }
    }
}