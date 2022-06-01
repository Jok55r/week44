using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Agario
{
    internal class Food
    {
        public const int size = 10;
        public CircleShape foodObj = new CircleShape(size, 10);

        public Food(RenderWindow win, Vector2f pos, Color col)
        {
            foodObj.FillColor = col;
            foodObj.Position = pos;
        }
    }
}