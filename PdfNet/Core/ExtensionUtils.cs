using System.Drawing;
using System.Numerics;

namespace PdfNet.Core;

public static class ExtensionUtils
{
    public static Vector2 Size(this RectangleF rectangle)
    {
        return new Vector2(rectangle.Size.Width, rectangle.Size.Height);
    }
}