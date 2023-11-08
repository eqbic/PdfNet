using System;
using System.Drawing;
using PDFiumCore;

namespace PdfNet.Unsafe
{
    public static class UnsafeUtils
    {
        public static void RenderPage(byte[] data, Rectangle viewport, Rectangle pageRectangle, FpdfPageT page)
        {
            var renderRectangle = Rectangle.Intersect(pageRectangle, viewport);
            var startPos = pageRectangle.Y - viewport.Y;
            var stride = renderRectangle.Width * 4;
            var firstLineOffset = Math.Max(startPos * stride, 0);
            try
            {
                unsafe
                {
                    fixed (byte* pointer = data)
                    {
                        IntPtr ptr = (IntPtr)pointer;
                        var bufferHandle = fpdfview.FPDFBitmapCreateEx(renderRectangle.Width, renderRectangle.Height, (int)FPDFBitmapFormat.BGRA, ptr + firstLineOffset, stride);
                        try
                        {
                            uint background = 0xFFFFFFFF;
                            fpdfview.FPDFBitmapFillRect(bufferHandle, 0, 0, renderRectangle.Width, renderRectangle.Height, background);
                            fpdfview.FPDF_RenderPageBitmap(bufferHandle, page, pageRectangle.X, startPos < 0 ? startPos : 0, pageRectangle.Width, pageRectangle.Height, 0, 0);
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

        public static FpdfDocumentT LoadRaw(byte[] data, string password = "")
        {
            try
            {
                unsafe
                {
                    fixed (byte* pointer = data)
                    {
                        IntPtr ptr = (IntPtr)pointer;
                        return fpdfview.FPDF_LoadMemDocument(ptr, data.Length, password);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static IntPtr GetPointer(byte[] data)
        {
            try
            {
                unsafe
                {
                    fixed (byte* pointer = data)
                    {
                        return (IntPtr)pointer;
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