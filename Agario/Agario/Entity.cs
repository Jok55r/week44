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
            key = aKey;
            pressed = false;
        }
    }

    internal class Entity : Circle
    {
        readonly private Randomchyk randomchyk = new Randomchyk();
        readonly private Buttons button = new Buttons(Keyboard.Key.R);

        private int howManyEat = 0;
        private float speed = 1f;
        private bool isPlayer = false;
        private bool goingForFood = false;
        private int toWhichIsGoing = 0;
        private Vector2f centre = new Vector2f(0, 0);

        public int lastFoodAtes = 0;

        public Entity(Vector2f pos, bool isPlayer, Color col, int rad)
        {
            shape = new CircleShape(rad, 1000)
            {
                Position = pos,
                FillColor = col,
                OutlineColor = Color.White,
                OutlineThickness = 2
            };
            shape.Position += new Vector2f(shape.Radius, shape.Radius);
            this.isPlayer = isPlayer;
        }

        void Move(Food[] food)
        {
            centre = shape.Position + new Vector2f(shape.Radius, shape.Radius);

            Vector2f d = new Vector2f(0, 0);

            Vector2f oldPos = centre;
            Vector2f newPos = (Vector2f)Mouse.GetPosition();

            if (!isPlayer && !goingForFood)
            {
                toWhichIsGoing = randomchyk.RandNum(0, Food.howManyFood);

                newPos = food[toWhichIsGoing].shape.Position;

                var thread1 = new Thread(new ThreadStart(() => BotChangePosition()));

                goingForFood = true;
            }
            else if (!isPlayer)
                newPos = food[toWhichIsGoing].shape.Position;

            if (newPos.X - oldPos.X < 0) d.X = -speed;
            if (newPos.X - oldPos.X > 0) d.X = +speed;
            if (newPos.Y - oldPos.Y < 0) d.Y = -speed;
            if (newPos.Y - oldPos.Y > 0) d.Y = +speed;

            shape.Position = oldPos + d - new Vector2f(shape.Radius, shape.Radius);
        }

        void BotChangePosition()
        {
            Thread.Sleep(2000);
            goingForFood = false;
        }

        void NewSize(Food[] food, Entity[] bots)
        {
            if (LookIfAte(food, bots))
            {
                shape.Radius += howManyEat;
                speed -= howManyEat * 0.002f;
            }
        }

        bool IsCloseEnough(Vector2f obj1, Vector2f obj2, Vector2f smaller)
        {
            return Math.Abs(obj1.X - obj2.X) < smaller.X &&
                   Math.Abs(obj1.Y - obj2.Y) < smaller.Y;
        }

        bool LookIfAte(Food[] food, Entity[] entities)
        {
            Vector2f radiusVect = new Vector2f(shape.Radius, shape.Radius);

            for (int i = 0; i < food.Length; i++)
            {
                if (IsCloseEnough(centre, food[i].shape.Position, radiusVect))
                {
                    howManyEat = (int)(food[i].shape.Radius / 10);
                    food[i] = new Food(randomchyk.RandVect(), randomchyk.RandColor());
                    lastFoodAtes = i;
                    return true;
                }
            }
            for (int i = 0; i < entities.Length; i++)
            {
                if (IsCloseEnough(centre, entities[i].centre, radiusVect) 
                    && shape.Radius > entities[i].shape.Radius)
                {
                    howManyEat = (int)(entities[i].shape.Radius / 4);
                    if (!entities[i].isPlayer) 
                        entities[i] = new Entity(randomchyk.RandVect(), false, randomchyk.RandColor(), randomchyk.RandNum(15, 40));
                    else
                        entities[i] = new Entity(randomchyk.RandVect(), true, Color.White, randomchyk.RandNum(15, 40));
                    return true;
                }
            }
            return false;
        }

        public void TryChangePlayer(Entity[] entities)
        {
            if (Keyboard.IsKeyPressed(button.key) && !button.pressed)
            {
                Vector2f howFar = new Vector2f(float.MaxValue, float.MaxValue);

                int whoNeedToChange = 0;

                for (int i = 0; i < entities.Length; i++)
                {
                    if (entities[i] != this && IsCloseEnough((Vector2f)Mouse.GetPosition(), entities[i].centre, howFar))
                    {
                        howFar.X = Math.Abs(Mouse.GetPosition().X - entities[i].shape.Position.X);
                        howFar.Y = Math.Abs(Mouse.GetPosition().Y - entities[i].shape.Position.Y);
                        /*howFar = Math.Abs(Mouse.GetPosition() - entities[i].shape.Position);*/
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

        public void Update(Food[] food, Entity[] entities)
        {
            Move(food);
            NewSize(food, entities);
            if (isPlayer) TryChangePlayer(entities);

            Global.win.Draw(shape);
        }
    }
}