using PdfNet.Core;
using SkiaSharp;
using Stride.Core.Mathematics;

namespace VL.PDF;

public static class PdfViewer
{
    public static PdfDocument LoadDocument(string path, string password = "")
    {
        return new PdfDocument(path, password);
    }

    public static void UpdatePageSizes(PdfDocument document, PdfViewport viewport)
    {
        document.UpdatePageSizes(viewport);
    }

    public static SKImage RenderDocument(PdfDocument document, PdfViewport viewport, SkiaTexture texture)
    {
        document.Render(viewport, texture);
        return texture.Image;
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
