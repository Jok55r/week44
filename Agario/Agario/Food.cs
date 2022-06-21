﻿using SFML.Graphics;
using SFML.System;

namespace Agario
{
    internal class Food : Ball
    {
        public static int howManyFood = 4000 / Global.scale;
        public const int size = Global.scale;

        public Food(Vector2f pos, Color col)
        {
            shape = new CircleShape(size, 10)
            {
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