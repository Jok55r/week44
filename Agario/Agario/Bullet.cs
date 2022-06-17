using SFML.Graphics;
using SFML.System;
using System.Threading;

namespace Agario
{
    internal class Bullet : Circle
    {
        private Vector2f d;

        public Bullet(Vector2f startPos, Vector2f d, bool shot)
        {
            shape = new CircleShape()
            {
                Position = startPos,
                Radius = 20,
                FillColor = Color.Black,
                OutlineColor = Color.Red,
                OutlineThickness = 5,
            };

            this.d = d;

            var thread1 = new Thread(new ThreadStart(() => Destroy(shot)));
            thread1.Start();
        }

        public void Move()
        {
            if (shape != null)
                shape.Position += d;
        }

        void Destroy(bool shot)
        {
            Thread.Sleep(3000);
            shape = null;
            shot = false;
        }
    }
}