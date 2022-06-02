using SFML.Graphics;
using SFML.System;

namespace Agario
{
    internal class Food
    {
        public int howManyFood = 250;
        public const int size = 10;
        public CircleShape foodObj = new CircleShape(size, 10);

        public Food(Vector2f pos, Color col)
        {
            foodObj.FillColor = col;
            foodObj.Position = pos;
        }

        public void Update(RenderWindow win)
        {
            win.Draw(this.foodObj);
        }
    }
}