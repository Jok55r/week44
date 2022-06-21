using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Rnd
    {
        readonly private static Random rnd = new Random();

        public static Vector2f RandVect() 
            => new Vector2f(rnd.Next(0, (int)Global.win.Size.X), rnd.Next(0, (int)Global.win.Size.Y));

        public static Color RandColor() 
            => new Color((byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255));

        public static int RandNum(int min, int max) 
            => rnd.Next(min, max);
    }
}