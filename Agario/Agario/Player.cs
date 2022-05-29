using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Agario
{
    internal class Player
    {
        Random rnd = new Random();
        public List<Player> players = new List<Player>();

        public int lastFoodAtes = 0;
        public float size = 20;
        public float xDir = 0;
        public float yDir = 0;
        public float speed = 1f;
        public CircleShape playerObj = new CircleShape();

        public Player()
        {
            //Player player = new Player();
            playerObj = new CircleShape(size, 1000);
            playerObj.Position = new Vector2f(0, 0);
            playerObj.FillColor = Color.Yellow;
            //players.Add(pla);
            //SpawnPlayer(playerObj.Position, size);
        }

        public void Move()
        {
            Vector2f oldMousePos = playerObj.Position;
            Vector2f mousePos = new Vector2f(Mouse.GetPosition().X, Mouse.GetPosition().Y);

            if (mousePos.X - oldMousePos.X < 0) xDir = -speed;
            if (mousePos.X - oldMousePos.X > 0) xDir = +speed;
            if (mousePos.Y - oldMousePos.Y < 0) yDir = -speed;
            if (mousePos.Y - oldMousePos.Y > 0) yDir = +speed;

            playerObj.Position = new Vector2f(oldMousePos.X + xDir, oldMousePos.Y + yDir);
        }

        public void NewSize(Food[] food, RenderWindow win)
        {
            if (AteFood(food, win))
            {
                size++;
                speed *= 0.99f;
            }

            playerObj.Radius = size;
        }

        public bool AteFood(Food[] food, RenderWindow win)
        {
            for (int i = 0; i < food.Length; i++)
            {
                if (Math.Abs(playerObj.Position.X - food[i].foodObj.Position.X + size / 2) < size &&
                    Math.Abs(playerObj.Position.Y - food[i].foodObj.Position.Y + size / 2) < size)
                {
                    food[i] = new Food(win, new Vector2f(rnd.Next(0, (int)win.Size.X), rnd.Next(0, (int)win.Size.Y)),
                       new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256)));
                    lastFoodAtes = i;
                    return true;
                }
            }
            return false;
        }

        public void Duplicate()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                size /= 2;
            }
        }

        public void SpawnPlayer(Vector2f pos, float size)
        {
            Player player = new Player();
            player.playerObj = new CircleShape(size, 1000);
            player.playerObj.FillColor = Color.Yellow;
            player.playerObj.Position = pos;
            player.playerObj.Radius = size;
            players.Add(player);
        }

        public void Update(Food[] food, RenderWindow win)
        {
            Move();
            NewSize(food, win);
            Duplicate();

            win.Draw(playerObj);
        }
    }
}