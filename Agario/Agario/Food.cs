using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Food
    {
        Player player = new Player();
        Random rnd = new Random();

        public int size = 10;
        public int howMany = 100;
        public CircleShape[] foodObj;

        public Food(RenderWindow window)
        {
            foodObj = new CircleShape[howMany];

            for (int i = 0; i < howMany; i++)
            {
                foodObj[i] = new CircleShape(size, 10);
                foodObj[i].FillColor = Color.White;
                NewPos(window, i);
            }
        }

        public void NewPos(RenderWindow window, int i)
        {
            foodObj[i].Position = new Vector2f(rnd.Next(0, (int)window.Size.X), rnd.Next(0, (int)window.Size.Y));
        }

        public void Update(RenderWindow window)
        {
            for (int i = 0; i < howMany; i++)
            {
                window.Draw(foodObj[i]);
            }
            window.Display();

            if (player.AteFood(window))
            {
                NewPos(window, player.lastFoodAtes);
            }
        }
    }
}