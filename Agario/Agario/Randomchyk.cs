﻿using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Randomchyk
    {
        Random rnd = new Random();

        public Vector2f RandVect(Vector2u winSize) 
            => new Vector2f(rnd.Next(0, (int)winSize.X), rnd.Next(0, (int)winSize.Y));

        public Color RandColor() 
            => new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256));

        public int RandNum(int min, int max) 
            => rnd.Next(min, max);
    }
}