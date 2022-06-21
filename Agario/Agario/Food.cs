namespace Agario
{
    internal class Food : Ball
    {
        public static int howManyFood = 4000 / Global.scale;
        public const int size = Global.scale;

        public Food()
        {
            SetBall(Rnd.RandColor(), size, 0);
            SpawnBall();
        }

        public void Update()
        {
            Global.win.Draw(shape);
        }
    }
}