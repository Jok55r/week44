using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Threading;

namespace Agario
{
    internal class Entity : Circle
    {
        private const int howFatNeedToBe = 500;

        readonly private Randomchyk randomchyk = new Randomchyk();
        readonly private Bullet bullet = new Bullet();

        private bool isSpacePressed = false;

        readonly public bool isPlayer = false;
        private float speed = 1f;
        readonly private int toWhichIsGoing = 0;
        public Vector2f centre = new Vector2f(0, 0);

        public Entity(bool isPlayer, Color col)
        {
            shape = new CircleShape(2 * Global.scale, 100)
            {
                Position = randomchyk.RandVect(),
                FillColor = col,
                OutlineColor = Color.White,
                OutlineThickness = 2
            };
            shape.Position += new Vector2f(shape.Radius, shape.Radius);
            this.isPlayer = isPlayer;

            if (!isPlayer)
                toWhichIsGoing = randomchyk.RandNum(0, Food.howManyFood);
        }

        void Move(Food[] food)
        {
            centre = shape.Position + new Vector2f(shape.Radius, shape.Radius);

            Vector2f d = new Vector2f(0, 0);

            Vector2f oldPos = centre;
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
                shape.Radius = randomchyk.RandNum(15, 40);
                shape.Position = centre;
            }
        }

        void Eat(float howManyEat)
        {
            shape.Radius += howManyEat;
            speed = 1 / howManyEat;
        }

        public static bool IsIn(Vector2f obj1, Vector2f obj2, Vector2f smaller)
        {
            return Math.Abs(obj1.X - obj2.X) < smaller.X &&
                   Math.Abs(obj1.Y - obj2.Y) < smaller.Y;
        }

        /*int LoopForEating(int length, Entity[] entities, Food[] food)
        {
            Vector2f radiusVect = new Vector2f(shape.Radius, shape.Radius);

            for (int i = 0; i < length; i++)
            {
                if (IsIn(centre, entities[i].centre, radiusVect) && shape.Radius > entities[i].shape.Radius)
                    return 1000 + i;

                if (IsIn(centre, food[i].shape.Position, radiusVect))
                    return 2000 + i;
            }

            return 0;
        }*/

        bool LookIfAte(Food[] food, Entity[] entities)
        {
            /*int num = LoopForEating(entities.Length, entities, food);

            switch (num)
            {
                case 0: return false;
                case > 1000:
                    if (entities[num - 1000].isPlayer)
                        entities[num - 1000] = new Entity(true, Color.White);
                    else
                        entities[num - 1000] = new Entity(false, randomchyk.RandColor());

                    Eat(entities[num - 1000].shape.Radius / 2);
                    return true;
            }

            if (LoopForEating(entities.Length, entities, food) == 0)
                return false;

            if (LoopForEating(entities.Length, entities, food) == 1)
                return true;*/

            Vector2f radiusVect = new Vector2f(shape.Radius, shape.Radius);

            for (int i = 0; i < entities.Length; i++)
            {
                if (IsIn(centre, entities[i].centre, radiusVect)
                    && shape.Radius > entities[i].shape.Radius)
                {
                    if (entities[i].isPlayer)
                        entities[i] = new Entity(true, Color.White);
                    else
                        entities[i] = new Entity(false, randomchyk.RandColor());

                    Eat(entities[i].shape.Radius / 2);
                    return true;
                }
            }
            for (int i = 0; i < food.Length; i++)
            {
                if (IsIn(centre, food[i].shape.Position, radiusVect))
                {
                    food[i] = new Food(randomchyk.RandVect(), randomchyk.RandColor());

                    Eat(Food.size / 5);
                    return true;
                }
            }
            return false;
        }

        void TryShoot(Vector2f d)
        {
            if (!isSpacePressed && Keyboard.IsKeyPressed(Keyboard.Key.Space) && shape.Radius > bullet.mass * 2)
            {
                shape.Radius -= bullet.mass;
                bullet.Shoot(centre + d * shape.Radius, new Vector2f((int)(d.X * 5), (int)(d.Y * 5)));
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