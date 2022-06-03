using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Agario
{
    internal class Entity
    {
        Random rnd = new Random();
        Randomchyk randomchyk = new Randomchyk();

        int howManyEat = 0;
        float xDir = 0;
        float yDir = 0;
        float speed = 1f;
        bool isPlayer = false;
        bool goingForFood = false;
        int towhichIsGoing = 0;

        public List<Entity> playerList = new List<Entity>();
        bool isPressed = false;
        public int lastFoodAtes = 0;
        public int size = 20;
        public CircleShape entityObj = new CircleShape();

        public Entity(Vector2f pos, bool isPlayer, Color col, int rad)
        {
            if (isPlayer)
                playerList.Add(this);

            entityObj = new CircleShape(rad, 1000);
            entityObj.Position = pos;
            entityObj.FillColor = col;

            this.isPlayer = isPlayer;
        }

        void Move(Food[] food)
        {
            Vector2f oldPos = entityObj.Position;
            Vector2f newPos = new Vector2f(Mouse.GetPosition().X, Mouse.GetPosition().Y);

            if (!isPlayer && !goingForFood)
            {
                int num = randomchyk.RandNum(0, food[0].howManyFood);
                towhichIsGoing = num;
                newPos = food[num].foodObj.Position;
                var thread1 = new Thread(new ThreadStart(() => ChangePosition(this)));
                goingForFood = true;
            }
            else if (!isPlayer)
                newPos = food[towhichIsGoing].foodObj.Position;

            if (newPos.X - oldPos.X < 0) xDir = -speed;
            if (newPos.X - oldPos.X > 0) xDir = +speed;
            if (newPos.Y - oldPos.Y < 0) yDir = -speed;
            if (newPos.Y - oldPos.Y > 0) yDir = +speed;

            entityObj.Position = new Vector2f(oldPos.X + xDir, oldPos.Y + yDir);
        }

        void ChangePosition(Entity bot)
        {
            Thread.Sleep(2000);
            goingForFood = false;
        }

        void NewSize(Food[] food, RenderWindow win, Entity[] bots)
        {
            if (LookIfAte(food, win, bots))
            {
                size += howManyEat;
                speed -= howManyEat * 0.002f;
            }
            entityObj.Radius = size;
        }

        bool isCloseEnough(CircleShape obj1, CircleShape obj2, int needsToBeSmallerX, int needsToBeSmallerY)
        {
            return Math.Abs(obj1.Position.X - obj2.Position.X + obj1.Radius / 2) < needsToBeSmallerX &&
                   Math.Abs(obj1.Position.Y - obj2.Position.Y + obj1.Radius / 2) < needsToBeSmallerY;
        }

        bool LookIfAte(Food[] food, RenderWindow win, Entity[] bots)
        {
            for (int i = 0; i < food.Length; i++)
            {
                if (isCloseEnough(this.entityObj, food[i].foodObj, (int)this.size, (int)this.size))
                {
                    howManyEat = (int)(food[i].foodObj.Radius / 10);
                    food[i] = new Food(randomchyk.RandVect(win), randomchyk.RandColor());
                    lastFoodAtes = i;
                    return true;
                }
            }
            for (int i = 0; i < bots.Length; i++)
            {
                if (isCloseEnough(this.entityObj, bots[i].entityObj, (int)this.size / 2, (int)this.size / 2) 
                    && entityObj.Radius > bots[i].entityObj.Radius)
                {
                    howManyEat = (int)(bots[i].entityObj.Radius / 4);
                    bots[i] = new Entity(randomchyk.RandVect(win), false, randomchyk.RandColor(), size);
                    return true;
                }
            }
            return false;
        }

        public void TryDuplicate(RenderWindow win)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && !isPressed && size >= 40 && playerList.Count < 9)
            {
                size /= 2;
                speed += size * 0.01f;
                isPressed = true;

                Vector2f newPos = new Vector2f(this.entityObj.Position.X + xDir * this.size, this.entityObj.Position.Y + yDir * this.size);

                playerList.Add(new Entity(newPos, true, this.entityObj.FillColor, (int)this.entityObj.Radius));
                var threa1 = new Thread(new ThreadStart(() => DestroyCopy(playerList[1])));
            }
            else if (!Keyboard.IsKeyPressed(Keyboard.Key.Space))
                isPressed = false;
        }

        public void DestroyCopy(Entity playerCopy)
        {
            Thread.Sleep(1000);
            playerList.Remove(playerCopy);
        }

        public void Update(Food[] food, RenderWindow win, Entity[] bots)
        {
            Move(food);
            NewSize(food, win, bots);
            if (isPlayer) TryDuplicate(win);

            win.Draw(this.entityObj);
        }
    }
}