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
        private Bullet bullet;

        private bool isSpacePressed = false;
        private bool shot = false;

        readonly private bool isPlayer = false;
        private int howManyEat = 0;
        private float speed = 1f;
        private bool goingForFood = false;
        private int toWhichIsGoing = 0;

        private Vector2f centre = new Vector2f(0, 0);

        public Entity(bool isPlayer, Color col)
        {
            shape = new CircleShape(randomchyk.RandNum(15, 40), 100)
            {
                Position = randomchyk.RandVect(),
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
            if (newPos.X - oldPos.X > 0) d.X = speed;
            if (newPos.Y - oldPos.Y < 0) d.Y = -speed;
            if (newPos.Y - oldPos.Y > 0) d.Y = speed;

            shape.Position = oldPos + d - new Vector2f(shape.Radius, shape.Radius);

            if (isPlayer)
                TryShoot(d);

            if (bullet != null)
                bullet.Move();
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

                if (shape.Radius > howFatNeedToBe)
                {
                    speed += shape.Radius * 0.002f;
                    shape.Radius = randomchyk.RandNum(15, 40);
                    shape.Position = centre;
                }
            }
        }

        bool IsIn(Vector2f obj1, Vector2f obj2, Vector2f smaller)
        {
            return Math.Abs(obj1.X - obj2.X) < smaller.X &&
                   Math.Abs(obj1.Y - obj2.Y) < smaller.Y;
        }

        bool LookIfAte(Food[] food, Entity[] entities)
        {
            Vector2f radiusVect = new Vector2f(shape.Radius, shape.Radius);

            for (int i = 0; i < food.Length; i++)
            {
                if (IsIn(centre, food[i].shape.Position, radiusVect))
                {
                    food[i] = new Food(randomchyk.RandVect(), randomchyk.RandColor());

                    howManyEat = Food.size / 10;
                    return true;
                }
            }
            for (int i = 0; i < entities.Length; i++)
            {
                if (IsIn(centre, entities[i].centre, radiusVect) 
                    && shape.Radius > entities[i].shape.Radius)
                {
                    if (entities[i].isPlayer) 
                        entities[i] = new Entity(true, Color.White);
                    else
                        entities[i] = new Entity(false, randomchyk.RandColor());

                    howManyEat = (int)(entities[i].shape.Radius / 4);
                    return true;
                }
            }
            return false;
        }

        void TryShoot(Vector2f d)
        {
            if (!isSpacePressed && Keyboard.IsKeyPressed(Keyboard.Key.Space) && shape.Radius > 40)
            {
                shape.Radius -= 20;
                bullet = new Bullet(centre + d * shape.Radius, new Vector2f((int)(d.X * 10), (int)(d.Y * 10)), shot);
                isSpacePressed = true;
                shot = true;
            }

            else if (isSpacePressed && !Keyboard.IsKeyPressed(Keyboard.Key.Space))
                isSpacePressed = false;
        }

        void LookIfShotSomeone(Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].isPlayer)
                    continue;

                if (IsIn(entities[i].centre, bullet.shape.Position, 
                    new Vector2f(shape.Radius, shape.Radius)))
                {
                    bullet.shape = null;
                    entities[i].shape.Radius /= 2;
                    break;
                }
            }
        }

        public void Update(Food[] food, Entity[] entities)
        {
            Move(food);
            NewSize(food, entities);
            if (shot && bullet.shape != null) 
                LookIfShotSomeone(entities);

            if (shot && bullet.shape != null)
                Global.win.Draw(bullet.shape);

            Global.win.Draw(shape);
        }
    }
}