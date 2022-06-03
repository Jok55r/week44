using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Randomchyk
    {
        Random rnd = new Random();

        public Vector2f RandVect(RenderWindow win) => new Vector2f(rnd.Next(0, (int)win.Size.X), rnd.Next(0, (int)win.Size.Y));

        public Color RandColor() => new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256));

        public int RandNum(int min, int max) => rnd.Next(min, max);
    }
}