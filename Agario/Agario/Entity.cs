using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Agario
{
    internal class Entity : Ball
    {
        private const float maxSpeed = 1f;
        private const float minSpeed = 0.2f;

        readonly private Bullet bullet = new Bullet();

        private const int howFatNeedToBe = 500;

        private readonly Text text = new Text("", new Font(@"D:\Github\week44\Agario\Agario\shrift.ttf"));
        private int score = 0;
        private bool isSpacePressed = false;
        readonly public bool isPlayer = false;
        readonly private int toWhichIsGoing = 0;

        public Entity(bool isPlayer)
        {
            SetBall(Color.Black, 2 * Global.scale, 2);
            SpawnBall();
            shape.Position += new Vector2f(shape.Radius, shape.Radius);

            this.isPlayer = isPlayer;

            if (!isPlayer)
            {
                shape.FillColor = Rnd.RandColor();
                toWhichIsGoing = Rnd.RandNum(0, Food.howManyFood);
            }
            text.DisplayedString = "0";
            text.Scale = new Vector2f(10, 10);
        }
        public override void SpawnBall()
        {
            base.SpawnBall();
            shape.Radius = 2 * Global.scale;
            speed = 1;
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
            text.Position = Centre() - new Vector2f(0, 300);

            if (isPlayer)
                TryShoot(d);
        }

        void NewSize()
        {
            if (shape.Radius > howFatNeedToBe)
            {
                Vector2f pos = Centre();
                SpawnBall();
                shape.Position = pos;

                score++;
                text.DisplayedString = score.ToString();
            }
        }

        void Eat(float howManyEat)
        {
            shape.Radius += howManyEat;
            speed = 1 / (shape.Radius / 30);
            if (speed < minSpeed)
                speed = minSpeed;
            else if (speed > maxSpeed)
                speed = maxSpeed;
        }

        public static bool IsIn(Vector2f obj1, Vector2f obj2, float smaller)
        {
            return Math.Abs(obj1.X - obj2.X) < smaller &&
                   Math.Abs(obj1.Y - obj2.Y) < smaller;
        }

        Tuple<bool, int> LoopForEating(int length, List<Ball> circle)
        {
            for (int i = 0; i < length; i++)
            {
                if (IsIn(Centre(), circle[i].Centre(), shape.Radius) && shape.Radius > circle[i].shape.Radius)
                    return Tuple.Create(true, i);
            }
            return Tuple.Create(false, 0);
        }

        bool LookIfAte(List<Ball> eatable)
        {
            Tuple<bool, int> result = LoopForEating(eatable.Count, eatable);

            if (result.Item1)
            {
                eatable[result.Item2].SpawnBall();

                Eat(eatable[result.Item2].shape.Radius / 4);
                return true;
            }

            return false;
        }

        void TryShoot(Vector2f d)
        {
            if (!isSpacePressed && Keyboard.IsKeyPressed(Keyboard.Key.Space) && shape.Radius > bullet.mass * 2)
            {
                shape.Radius -= bullet.mass;
                bullet.Shoot((Vector2i)(Centre() + d * shape.Radius));
                isSpacePressed = true;
                bullet.shot = true;
            }
            else if (isSpacePressed && !Keyboard.IsKeyPressed(Keyboard.Key.Space))
                isSpacePressed = false;
        }

        public void Update(Food[] food, Entity[] entities, List<Ball> eatable)
        {
            Move(food);
            NewSize();
            LookIfAte(eatable);

            if (bullet.shot)
                bullet.Update(entities);

            Global.win.Draw(shape);
            Global.win.Draw(text);
        }
    }
}