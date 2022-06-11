using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Agario
{
    internal class Buttons
    {
        public Keyboard.Key key;
        public bool pressed;

        public Buttons(Keyboard.Key aKey)
        {
            this.key = aKey;
            pressed = false;
        }
    }

    internal class Entity
    {
        private Randomchyk randomchyk = new Randomchyk();
        private Buttons button = new Buttons(Keyboard.Key.R);

        private int howManyEat = 0;
        private float speed = 1f;
        private bool isPlayer = false;
        private bool goingForFood = false;
        private int toWhichIsGoing = 0;
        //private bool isPressedR = false;

        public List<Entity> playerList = new List<Entity>();
        public int lastFoodAtes = 0;
        public int size = 20;
        public CircleShape entityObj = new CircleShape();

        public Entity(Vector2f pos, bool isPlayer, Color col, int rad)
        {
            if (isPlayer)
                playerList.Add(this);

            entityObj = new CircleShape(rad, 1000)
            {
                Position = pos,
                FillColor = col
            };

            this.isPlayer = isPlayer;
        }

        void Move(Food[] food)
        {
            float dx = 0;
            float dy = 0;
            Vector2f oldPos = entityObj.Position;
            Vector2f newPos = new Vector2f(Mouse.GetPosition().X, Mouse.GetPosition().Y);

            if (!isPlayer && !goingForFood)
            {
                int num = randomchyk.RandNum(0, food[0].howManyFood);
                toWhichIsGoing = num;
                newPos = food[num].foodObj.Position;
                var thread1 = new Thread(new ThreadStart(() => ChangePosition()));
                goingForFood = true;
            }
            else if (!isPlayer)
                newPos = food[toWhichIsGoing].foodObj.Position;

            if (newPos.X - oldPos.X < 0) dx = -speed;
            if (newPos.X - oldPos.X > 0) dx = +speed;
            if (newPos.Y - oldPos.Y < 0) dy = -speed;
            if (newPos.Y - oldPos.Y > 0) dy = +speed;

            entityObj.Position = new Vector2f(oldPos.X + dx, oldPos.Y + dy);
        }

        void ChangePosition()
        {
            Thread.Sleep(2000);
            goingForFood = false;
        }

        void NewSize(Food[] food, Vector2u winSize, Entity[] bots)
        {
            if (LookIfAte(food, winSize, bots))
            {
                size += howManyEat;
                speed -= howManyEat * 0.002f;
            }
            entityObj.Radius = size;
        }

        bool isCloseEnough(Vector2f obj1, Vector2f obj2, float needsToBeSmallerX, float needsToBeSmallerY)
        {
            return Math.Abs(obj1.X - obj2.X) < needsToBeSmallerX &&
                   Math.Abs(obj1.Y - obj2.Y) < needsToBeSmallerY;
        }

        bool LookIfAte(Food[] food, Vector2u winSize, Entity[] entities)
        {
            for (int i = 0; i < food.Length; i++)
            {
                if (isCloseEnough(entityObj.Position, food[i].foodObj.Position, (int)this.size, (int)this.size))
                {
                    howManyEat = (int)(food[i].foodObj.Radius / 10);
                    food[i] = new Food(randomchyk.RandVect(winSize), randomchyk.RandColor());
                    lastFoodAtes = i;
                    return true;
                }
            }
            for (int i = 0; i < entities.Length; i++)
            {
                if (isCloseEnough(entityObj.Position, entities[i].entityObj.Position, (int)size / 2, (int)size / 2) 
                    && entityObj.Radius > entities[i].entityObj.Radius)
                {
                    howManyEat = (int)(entities[i].entityObj.Radius / 4);
                    entities[i] = new Entity(randomchyk.RandVect(winSize), false, randomchyk.RandColor(), size);
                    return true;
                }
            }
            return false;
        }

        public void TryChangePlayer(Entity[] entities)
        {
            if (Keyboard.IsKeyPressed(button.key) && !button.pressed)
            {
                float howFarX = int.MaxValue;
                float howFarY = int.MaxValue;
                int whoNeedToChange = 0;

                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i] != this && isCloseEnough((Vector2f)Mouse.GetPosition(), entities[i].entityObj.Position, howFarX, howFarY))
                    {
                        howFarX = Math.Abs(Mouse.GetPosition().X - entities[i].entityObj.Position.X);
                        howFarY = Math.Abs(Mouse.GetPosition().Y - entities[i].entityObj.Position.Y);
                        whoNeedToChange = i;
                    }
                    else if (entities[i] == this)
                        entities[i].isPlayer = false;
                }
                entities[whoNeedToChange].isPlayer = true;

                button.pressed = true;
            }
            else if (!Keyboard.IsKeyPressed(Keyboard.Key.R))
                button.pressed = false;
        }

        public void Update(Food[] food, RenderWindow win, Entity[] entities)
        {
            Move(food);
            NewSize(food, win.Size, entities);
            if (isPlayer) TryChangePlayer(entities);

            win.Draw(entityObj);
        }
    }
}