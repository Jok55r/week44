using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Player
    {
        public int lastFoodAtes = 0;
        public float size = 20;
        public uint oldMass = 10;
        public uint mass = 10;
        public float speed = 1.005f;
        public CircleShape playerObj = new CircleShape();

        public Player()
        {
            playerObj = new CircleShape(size, 1000);
            playerObj.FillColor = Color.Yellow;
        }

        public void Move()
        {
            Vector2f oldMousePos = playerObj.Position;
            Vector2f mousePos = new Vector2f(Mouse.GetPosition().X, Mouse.GetPosition().Y);

            float distanceX = mousePos.X - oldMousePos.X;
            float distanceY = mousePos.Y - oldMousePos.Y;

            playerObj.Position = new Vector2f(oldMousePos.X + distanceX - distanceX / speed, oldMousePos.Y + distanceY - distanceY / speed);
        }

        public void NewSize(RenderWindow window)
        {
            if (AteFood(window))
            {
                size += mass - oldMass;
                oldMass = mass;
                speed *= 0.999999f;
            }

            playerObj.Radius = size;
        }

        public bool AteFood(RenderWindow window)
        {
            Food food = new Food(window);

            for (int i = 0; i < food.howMany; i++)
            {
                if (Math.Abs(playerObj.Position.Y - food.foodObj[i].Position.Y) < 20 &&
                    Math.Abs(playerObj.Position.X - food.foodObj[i].Position.X) < 20)
                {
                    lastFoodAtes = i;
                    return true;
                }
            }
            return false;
        }

        public void Update(RenderWindow window)
        {
            Move();
            NewSize(window);

            window.Draw(playerObj);
            window.Display();
        }
    }
}