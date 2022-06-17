using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Randomchyk
    {
        readonly private Random rnd = new Random();

        public Vector2f RandVect() 
            => new Vector2f(rnd.Next(0, (int)Global.win.Size.X), rnd.Next(0, (int)Global.win.Size.Y));

        public Color RandColor() 
            => new Color((byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255));

        public int RandNum(int min, int max) 
            => rnd.Next(min, max);
    }
}