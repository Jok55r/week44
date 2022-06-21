using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using SFML.Window;
using System;

namespace Agario
{
    internal class Bullet : Ball
    {
        public float mass = 2 * Global.scale;
        public bool shot = false;
        readonly private List<Tuple<Vector2i, CircleShape>> ballz = new List<Tuple<Vector2i, CircleShape>>();

        public void Shoot(Vector2i startPos)
        {
            shape = new CircleShape(mass, 50)
            {
                FillColor = Color.Black,
                OutlineColor = Color.Red,
                OutlineThickness = 5,
            };
            ballz.Add(Tuple.Create(Mouse.GetPosition() - startPos, shape));

            speed = 0.01f;
        }

        public void Move()
        {
            for (int i = 0; i < ballz.Count; i++)
            {
                ballz[i].Item2.Position += (Vector2f)ballz[i].Item1 * speed;
            }
        }

        void TryDestroy()
        {
            if (ballz.Count > 5)
                ballz.RemoveAt(0);
        }

        void LookIfShotSomeone(Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].isPlayer)
                    continue;

                for (int j = 0; j < ballz.Count; j++)
                {
                    if (Entity.IsIn(entities[i].Centre(), ballz[j].Item2.Position,
                           new Vector2f(entities[i].shape.Radius, entities[i].shape.Radius)))
                    {
                        ballz.RemoveAt(j);
                        entities[i].shape.Radius /= 2;
                        break;
                    }
                }
            }
        }

        public void Update(Entity[] entities)
        {
            Move();
            TryDestroy();
            LookIfShotSomeone(entities);

            for (int i = 0; i < ballz.Count; i++)
                Global.win.Draw(ballz[i].Item2);
        }
    }
}