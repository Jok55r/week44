using SFML.Graphics;
using SFML.System;

namespace Agario
{
    internal class Food : Circle
    {
        public static int howManyFood = 250;
        public const int size = 10;

        public Food(Vector2f pos, Color col)
        {
            shape = new CircleShape()
            {
                Radius = size,
                FillColor = col,
                Position = pos,
            };
        }

        public void Update()
        {
            Global.win.Draw(shape);
        }
    }
}