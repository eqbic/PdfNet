using System;
using System.Drawing;
using PDFiumCore;

namespace PdfNet.Core
{
    public class PdfPage
    {
        public FpdfPageT Page { get; }


        private readonly float _aspectRatio;
        private int _startPositionY;

        private Rectangle _rectangle;
        public Rectangle Rectangle => _rectangle;
        private readonly int _index;

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

        private void CalculatePageSize(PdfViewport viewport)
        {
            _rectangle.Width = viewport.Size.X;
            _rectangle.Height = (int)(_rectangle.Width * _aspectRatio);
            _rectangle.Y = _index * _rectangle.Height;
        }

        public void UpdatePageSize(PdfViewport viewport)
        {
            CalculatePageSize(viewport);
        }

        public void Render(PdfViewport viewport)
        {
            var renderRectangle = Rectangle.Intersect(_rectangle, viewport.Rectangle);
            var startPos = _rectangle.Y - viewport.Rectangle.Y;
            var stride = renderRectangle.Width * PdfConstants.BytesPerPixel;
            var firstLineOffset = Math.Max(startPos * stride, 0);
            try
            {
                unsafe
                {
                    fixed (byte* pointer = viewport.Data)
                    {
                        IntPtr ptr = (IntPtr)pointer;
                        var bufferHandle = fpdfview.FPDFBitmapCreateEx(renderRectangle.Width,
                            renderRectangle.Height, (int)FPDFBitmapFormat.BGRA, ptr + firstLineOffset,
                            stride);
                        try
                        {
                            uint background = 0xFFFFFFFF;
                            fpdfview.FPDFBitmapFillRect(bufferHandle, renderRectangle.X, 0, renderRectangle.Width, renderRectangle.Height, background);
                            fpdfview.FPDF_RenderPageBitmap(bufferHandle, Page, -renderRectangle.X, startPos < 0 ? startPos : 0, _rectangle.Width, _rectangle.Height, 0, 0);
                        }
                        finally
                        {
                            fpdfview.FPDFBitmapDestroy(bufferHandle);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}