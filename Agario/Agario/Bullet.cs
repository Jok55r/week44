using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Agario
{
    internal class Bullet : Ball
    {
        public float mass = 2 * Global.scale;
        public bool shot = false;
        private Tuple<Vector2i, CircleShape> bullet;

        public void Shoot(Vector2f startPos)
        {
            shape = new CircleShape(mass, 50)
            {
                Position = startPos,
                FillColor = Color.Black,
                OutlineColor = Color.Red,
                OutlineThickness = 5,
            };
            bullet = Tuple.Create(Mouse.GetPosition() - (Vector2i)startPos, shape);

            speed = 0.01f;
        }

        public void Move()
            => bullet.Item2.Position += (Vector2f) bullet.Item1 * speed;

        void LookIfShotSomeone(Entity[] entities)
        {
            foreach (var entity in entities)
            {
                if (entity.isPlayer)
                    continue;

                if (Entity.IsIn(entity.Centre(), bullet.Item2.Position, entity.shape.Radius))
                {
                    //bullet = null;
                    entity.shape.Radius /= 2;
                    break;
                }
            }
        }

        public void Update(Entity[] entities)
        {
            Move();
            LookIfShotSomeone(entities);

            Global.win.Draw(bullet.Item2);
        }
    }
}