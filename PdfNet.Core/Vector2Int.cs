namespace PdfNet.Core
{
    public struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.X + b.X, a.Y + b.Y);
        public static Vector2Int operator -(Vector2Int a) => new Vector2Int(-a.X, -a.Y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
        public static Vector2Int operator *(Vector2Int a, int s) => new Vector2Int(a.X * s, a.Y * s);
        public static Vector2Int operator *(Vector2Int a, float s) => new Vector2Int((int)(a.X * s), (int)(a.Y * s));

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}