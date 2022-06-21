using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using SFML.Window;

namespace Agario
{
    internal class Bullet : Ball
    {
        public float mass = 2 * Global.scale;
        public bool shot = false;
        readonly private List<Vector2i> d = new List<Vector2i>();
        public List<CircleShape> ballz = new List<CircleShape>();

        public void Shoot(Vector2f startPos)
        {
            ballz.Add(shape = new CircleShape(mass, 50)
            {
                Position = startPos,
                FillColor = Color.Black,
                OutlineColor = Color.Red,
                OutlineThickness = 5,
            });
            d.Add(Mouse.GetPosition() - (Vector2i)startPos);

            speed = 0.01f;
        }

        public void Move()
        {
            for(int i = 0; i < ballz.Count; i++)
            {
                ballz[i].Position += (Vector2f)d[i] * speed;
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
                    if (Entity.IsIn(entities[i].Centre(), ballz[j].Position, 
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
        }
    }
}