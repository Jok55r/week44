using SFML.Graphics;
using SFML.System;

namespace Agario
{
    internal class Food : Ball
    {
        public static int howManyFood = 4000 / Global.scale;
        public const int size = Global.scale;

        public Food()
        {
            shape = new CircleShape(size, 10)
            {
                FillColor = Rnd.RandColor(),
                Position = Rnd.RandVect(),
            };
        }

        public void Update()
        {
            Global.win.Draw(shape);
        }
    }
}