using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;

namespace Agario
{
    internal class Entity : Ball
    {
        readonly private Bullet bullet = new Bullet();

        private const int howFatNeedToBe = 500;

        private bool isSpacePressed = false;
        private int num = 0;
        readonly public bool isPlayer = false;
        readonly private int toWhichIsGoing = 0;

        public Entity(bool isPlayer, Color col)
        {
            shape = new CircleShape(2 * Global.scale, 100)
            {
                Position = Randomchyk.RandVect(),
                FillColor = col,
                OutlineColor = Color.White,
                OutlineThickness = 2
            };
            shape.Position += new Vector2f(shape.Radius, shape.Radius);
            this.isPlayer = isPlayer;
            speed = 1;

            if (!isPlayer)
                toWhichIsGoing = Randomchyk.RandNum(0, Food.howManyFood);
        }

        void Move(Food[] food)
        {
            Vector2f d = new Vector2f(0, 0);

            Vector2f oldPos = Centre();
            Vector2f newPos = (Vector2f)Mouse.GetPosition();

            if (!isPlayer)
                newPos = food[toWhichIsGoing].shape.Position;

            if (newPos.X - oldPos.X < 0) d.X = -speed;
            if (newPos.X - oldPos.X > 0) d.X = speed;
            if (newPos.Y - oldPos.Y < 0) d.Y = -speed;
            if (newPos.Y - oldPos.Y > 0) d.Y = speed;

            shape.Position = oldPos + d - new Vector2f(shape.Radius, shape.Radius);

            if (isPlayer)
                TryShoot(d);
        }

        void NewSize()
        {
            if (shape.Radius > howFatNeedToBe)
            {
                speed = 1;
                shape.Radius = Randomchyk.RandNum(15, 40);
                shape.Position = Centre();
            }
        }

        void Eat(float howManyEat)
        {
            shape.Radius += howManyEat;
            speed = 1 / (shape.Radius / 30);
        }

        public static bool IsIn(Vector2f obj1, Vector2f obj2, Vector2f smaller)
        {
            return Math.Abs(obj1.X - obj2.X) < smaller.X &&
                   Math.Abs(obj1.Y - obj2.Y) < smaller.Y;
        }

        bool LoopForEating(int length, Ball[] circle)
        {
            for (int i = 0; i < length; i++)
            {
                if (IsIn(Centre(), circle[i].Centre(), new Vector2f(shape.Radius, shape.Radius)) && 
                    shape.Radius > circle[i].shape.Radius)
                {
                    num = i;
                    return true;
                }
            }
            return false;
        }

        bool LookIfAte(Food[] food, Entity[] entities)
        {
            if (LoopForEating(entities.Length, entities)) 
            {
                if (entities[num].isPlayer)
                    entities[num] = new Entity(true, Color.White);
                else
                    entities[num] = new Entity(false, Randomchyk.RandColor());

                Eat(entities[num].shape.Radius / 2);
                return true;
            }
            else if (LoopForEating(food.Length, food)) 
            {
                food[num] = new Food(Randomchyk.RandVect(), Randomchyk.RandColor());

                Eat(Food.size / 5);
                return true;
            }
            return false;
        }

        void TryShoot(Vector2f d)
        {
            if (!isSpacePressed && Keyboard.IsKeyPressed(Keyboard.Key.Space) && shape.Radius > bullet.mass * 2)
            {
                shape.Radius -= bullet.mass;
                bullet.Shoot(Centre() + d * shape.Radius);
                isSpacePressed = true;
                bullet.shot = true;
            }
            else if (isSpacePressed && !Keyboard.IsKeyPressed(Keyboard.Key.Space))
                isSpacePressed = false;
        }

        public void Update(Food[] food, Entity[] entities)
        {
            Move(food);
            NewSize();
            LookIfAte(food, entities);

            if (bullet.shot)
                bullet.Update(entities);

            if (bullet.shot)
            {
                for (int i = 0; i < bullet.ballz.Count; i++)
                    Global.win.Draw(bullet.ballz[i]);
            }

            Global.win.Draw(shape);
        }
    }
}