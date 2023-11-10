using System;
using System.Drawing;
using PDFiumCore;

namespace PdfNet.Unsafe
{
    public static class UnsafeUtils
    {
        public static void RenderPage(byte[] data, Rectangle viewport, Rectangle pageRectangle, FpdfPageT page, float zoom)
        {
            var renderRectangle = Rectangle.Intersect(pageRectangle, viewport);
            var startPos = pageRectangle.Y - viewport.Y;
            var stride = (int)(renderRectangle.Width * zoom * 4);
            var firstLineOffset = Math.Max(startPos * stride, 0);
            int width = (int)(renderRectangle.Width * zoom);
            int height = (int)(renderRectangle.Height * zoom);
            int pageWidth = (int)(pageRectangle.Width * zoom);
            int pageHeight = (int)(pageRectangle.Height * zoom);
            int offsetX = (int)(-renderRectangle.X * zoom);
            int offsetY = startPos < 0 ? (int)(startPos * zoom) : 0;
            try
            {
                unsafe
                {
                    fixed (byte* pointer = data)
                    {
                        IntPtr ptr = (IntPtr)pointer;
                        var bufferHandle = fpdfview.FPDFBitmapCreateEx(width, height, (int)FPDFBitmapFormat.BGRA, ptr + firstLineOffset, stride);
                        try
                        {
                            uint background = 0xFFFFFFFF;
                            fpdfview.FPDFBitmapFillRect(bufferHandle, 0, 0, width, height, background);
                            fpdfview.FPDF_RenderPageBitmap(bufferHandle, page, offsetX, offsetY, pageWidth, pageHeight, 0, 0);
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