using SFML.Graphics;
using SFML.System;

namespace Agario
{
    public class Ball
    {
        public CircleShape shape;
        public float speed = 0f;

        public Vector2f Centre()
        {
            return shape.Position + new Vector2f(shape.Radius, shape.Radius);
        }
    }
}