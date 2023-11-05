using PdfNet.Core;
using SkiaSharp;
using Stride.Core.Mathematics;

namespace VL.PDF
{
    public static class PdfViewer
    {
        public static PdfDocument<SKImage> LoadDocument(string path)
        {
            return new PdfDocument<SKImage>(path);
        }

        public static SKImage RenderDocument(PdfDocument<SKImage> document, PdfViewport<SKImage> viewport)
        {
            document.Render(viewport);
            return viewport.Image;
        }
        
        public static System.Numerics.Vector2 ToVec2 (Vector2 vector)
        {
            return new System.Numerics.Vector2(vector.X, vector.Y);
        }
    
        public static System.Numerics.Vector2 ToVec2 (Int2 vector)
        {
            return new System.Numerics.Vector2(vector.X, vector.Y);
        }
    }
}