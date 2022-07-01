using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Agario
{
    internal class Entity : Ball
    {
        private readonly Controller controller;
        private readonly List<Keyboard.Key> keys;

        private const float maxSpeed = 1f;
        private const float minSpeed = 0.2f;

        //private List<Bullet> bullet = new List<Bullet>();
        readonly private Bullet bullet = new Bullet();

        private const int howFatNeedToBe = 500;

        private readonly Text text = new Text("0", new Font(@"D:\Github\week44\Agario\Agario\font.ttf"));
        public int score = 0;
        private bool isSpacePressed = false;
        readonly public bool isPlayer = false;
        readonly private int toWhichIsGoing = 0;

        public Entity(bool isPlayer)
        {
            keys.Add(Keyboard.Key.Space);
            controller = new Controller(keys);

            SetBall(Color.Black, 2 * Global.scale, 2);
            SpawnBall();
            shape.Position += new Vector2f(shape.Radius, shape.Radius);

            this.isPlayer = isPlayer;
            if (!isPlayer)
            {
                shape.FillColor = Rnd.RandColor();
                toWhichIsGoing = Rnd.RandNum(0, Food.howManyFood);
            }
        }
        public override void SpawnBall()
        {
            base.SpawnBall();
            shape.Radius = 2 * Global.scale;
            speed = 1;
        }

        void Move(Food[] food)
        {
            Vector2f oldPos = Centre();
            Vector2f newPos = (Vector2f)Mouse.GetPosition();

            if (!isPlayer)
                newPos = food[toWhichIsGoing].shape.Position;

            Vector2f dist = oldPos - newPos;

            shape.Position = oldPos + (dist / speed);

            if (isPlayer)
                TryShoot();
        }

        void NewSize()
        {
            if (shape.Radius > howFatNeedToBe)
            {
                Vector2f pos = Centre();
                SpawnBall();
                shape.Position = pos;
                score++;
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
            => Math.Abs(obj1.X - obj2.X) < smaller &&
                   Math.Abs(obj1.Y - obj2.Y) < smaller;

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

        void TryShoot()
        {
            if (!isSpacePressed && controller.Pressed(keys[0]) && shape.Radius > bullet.mass * 2)
            {
                shape.Radius -= bullet.mass;
                bullet.Shoot(Centre() * shape.Radius);
                isSpacePressed = true;
                bullet.shot = true;
            }
            else if (isSpacePressed && !controller.Pressed(keys[0]))
            {
                isSpacePressed = false;
            }
        }

        void TextUpdate()
        {
            text.Position = Centre();
            text.DisplayedString = score.ToString();
            text.Scale = new Vector2f(shape.Radius / 50, shape.Radius / 50);
        }

        public void Update(Food[] food, Entity[] entities, List<Ball> eatable)
        {
            Move(food);
            NewSize();
            LookIfAte(eatable);
            TextUpdate();

            if (bullet.shot)
                bullet.Update(entities);

            Global.win.Draw(shape);
            Global.win.Draw(text);
        }
    }
}