using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Agario
{
    internal class Player
    {
        Random rnd = new Random();
        public List<Player> players = new List<Player>();

        public bool isPressed = false;
        public int lastFoodAtes = 0;
        public float size = 20;
        public float xDir = 0;
        public float yDir = 0;
        public float speed = 1f;
        public CircleShape playerObj = new CircleShape();

        public Player(Vector2f pos)
        {
            players.Add(this);
            playerObj = new CircleShape(size, 1000);
            playerObj.Position = pos;
            playerObj.FillColor = Color.Yellow;
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
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && !isPressed && size >= 40 && players.Count <= 8)
            {
                size /= 2;
                speed *= speed * 2f;
                isPressed = true;
                int j = players.Count;
                for (int i = 0; i < j; i++)
                {
                    players.Add(new Player(this.playerObj.Position));
                    DestroyCopy(players[players.Count - 1]);
                }
            }
            else if (!Keyboard.IsKeyPressed(Keyboard.Key.Space))
                isPressed = false;
        }

        public void DestroyCopy(Player playerCpoy)
        {
            /*Thread.Sleep(5000);
            players.Remove(playerCpoy);*/
        }

        public  void Update(Food[] food, RenderWindow win)
        {
            Move();
            NewSize(food, win);
            Duplicate();

            //win.Draw(playerObj);
        }
    }
}