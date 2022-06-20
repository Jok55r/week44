using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Agario
{
    internal class Bullet : Circle
    {
        public float mass = 2 * Global.scale;
        public bool shot = false;
        readonly private List<Vector2f> d = new List<Vector2f>();
        public List<CircleShape> ballz = new List<CircleShape>();

        public void Shoot(Vector2f startPos, Vector2f d)
        {
            ballz.Add(shape = new CircleShape(mass, 50)
            {
                Position = startPos,
                FillColor = Color.Black,
                OutlineColor = Color.Red,
                OutlineThickness = 5,
            });

            this.d.Add(d);
        }

        public void Move()
        {
            for(int i = 0; i < ballz.Count; i++)
            {
                ballz[i].Position += d[i];
            }
        }

        void TryDestroy()
        {
            if (ballz.Count > 5)
            {
                ballz.RemoveAt(0);
                d.RemoveAt(0);
            }
        }

        void LookIfShotSomeone(Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].isPlayer)
                    continue;

                for (int j = 0; j < ballz.Count; j++)
                {
                    if (Entity.IsIn(entities[i].centre, ballz[j].Position, new Vector2f(shape.Radius, shape.Radius)))
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
        }
    }
}