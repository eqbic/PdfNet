using System;
using System.Drawing;
using System.Numerics;
using PDFiumCore;

namespace PdfNet.Core
{
    public class PdfPage<T>
{
    private const float Dpi = 150 / 72.0f;

    public FpdfPageT Page { get; }


    private readonly float _aspectRatio;
    private int _startPositionY;

    private RectangleF _rectangle;
    public RectangleF Rectangle => _rectangle;
    private int _index;

    private Vector2 _nativeSize;
    
    public PdfPage(FpdfPageT page, int pageIndex)
    {
        Page = page;
        _index = pageIndex;
        var width = fpdfview.FPDF_GetPageWidthF(Page) * Dpi;
        var height = fpdfview.FPDF_GetPageHeightF(Page) * Dpi;
        _nativeSize = new Vector2(width, height);
        _aspectRatio = height / width;
        _startPositionY = (int)(pageIndex * height);
        _rectangle = new RectangleF(0f, _startPositionY, width, height);
    }

    private void CalculatePageSize(PdfViewport<T> viewport)
    {
        _rectangle.Width = Math.Min(_nativeSize.X, viewport.Size.X);
        _rectangle.Height = _rectangle.Width * _aspectRatio;
        _rectangle.Y = _index * _rectangle.Height;
    }

    public void UpdatePageSize(PdfViewport<T> viewport)
    {
        CalculatePageSize(viewport);
    }
    
    public void Render(PdfViewport<T> viewport)
    {
        var renderRectangle = RectangleF.Intersect(_rectangle, viewport.Rectangle);
        var startPos = (int)(_rectangle.Y - viewport.Rectangle.Y);
        Console.WriteLine($"Page {_index}: {renderRectangle}");
        var stride = (int)renderRectangle.Width * PdfConstants.BytesPerPixel;
        var firstLineOffset = Math.Max(startPos * stride, 0);
        try
        {
            unsafe
            {
                fixed (byte* pointer = viewport.Data)
                {
                    IntPtr ptr = (IntPtr)pointer;
                    var bufferHandle = fpdfview.FPDFBitmapCreateEx((int)renderRectangle.Width, (int)renderRectangle.Height, (int)FPDFBitmapFormat.BGRA, ptr + firstLineOffset,
                        stride);
                    try
                    {
                        uint background = 0xFFFFFFFF;
                        fpdfview.FPDFBitmapFillRect(bufferHandle, (int)renderRectangle.X, 0, (int)renderRectangle.Width, (int)renderRectangle.Height, background);
                        fpdfview.FPDF_RenderPageBitmap(bufferHandle, Page, -(int)renderRectangle.X, startPos < 0 ? startPos : 0, (int)_rectangle.Width, (int)_rectangle.Height, 0, 0);
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