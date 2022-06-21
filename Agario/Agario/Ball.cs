using SFML.Graphics;
using SFML.System;

namespace Agario
{
    public class Ball
    {
        public CircleShape shape;
        public float speed = 0f;

        public virtual void SpawnBall()
        {
            shape.Position = Rnd.RandVect();
        }

        public void SetBall(Color col, float rad, int thick)
        {
            shape = new CircleShape(rad, 100)
            {
                FillColor = col,
                OutlineColor = Color.White,
                OutlineThickness = thick,
            };
        }

        public Vector2f Centre()
            => shape.Position + new Vector2f(shape.Radius, shape.Radius);
    }
}