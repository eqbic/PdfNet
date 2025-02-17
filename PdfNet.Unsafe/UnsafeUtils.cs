﻿using System;
using System.Drawing;
using PDFiumCore;

namespace PdfNet.Unsafe
{
    public static class UnsafeUtils
    {
        public static void RenderPage(byte[] data, RectangleF viewport, RectangleF pageRectangle, FpdfPageT page, float zoom, float renderScale)
        {
            var scaleAmount = zoom * renderScale;
            var renderRectangle = RectangleF.Intersect(pageRectangle, viewport);
            var startPos = (int)Math.Round((pageRectangle.Y - viewport.Y) * scaleAmount);
            var stride = (int)Math.Round(renderRectangle.Width * scaleAmount) * 4;
            var firstLineOffset = Math.Max(startPos * stride, 0);
            int width = (int)Math.Round(renderRectangle.Width * scaleAmount);
            int height = (int)Math.Round(renderRectangle.Height * scaleAmount);
            int pageWidth = (int)Math.Round(pageRectangle.Width * scaleAmount);
            int pageHeight = (int)Math.Round(pageRectangle.Height * scaleAmount);
            int offsetX = (int)Math.Round(-renderRectangle.X * scaleAmount);
            int offsetY = startPos < 0 ? startPos : 0;
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