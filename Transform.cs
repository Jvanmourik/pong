using System;
using Microsoft.Xna.Framework;

namespace Pong
{
    public class Transform
    {
        public Vector2 position;
        public float Width;
        public float Height;

        public Vector2 size = new Vector2(1f, 1f);
        public Vector2 direction = new Vector2(0f, 0f);
        
        // Constructor
        public Transform(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            Width = width;
            Height = height;
        }

        // Return Rectangle object based on this transform
        public Rectangle getRectangle()
        {
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)Width,
                (int)Height);
        }
    }
}
