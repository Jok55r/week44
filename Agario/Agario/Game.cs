using SFML.Graphics;
using System.IO;
using System.Threading.Tasks;

namespace Agario
{
    internal class Game
    {
        readonly private Randomchyk randomchyk = new Randomchyk();
        readonly private Entity[] entities = new Entity[Global.howManyEntities];
        readonly private Food[] food = new Food[Food.howManyFood];

        public void GameStart()
        {
            var task = Task.Factory.StartNew(() => CreateFile());
            Task.WaitAll(task);

            Global.win.Closed += Global.WindowClosed;
            Global.win.SetFramerateLimit(Global.fps);

            for (int i = 0; i < Food.howManyFood; i++)
            {
                food[i] = new Food(randomchyk.RandVect(), randomchyk.RandColor());
            }

            for (int i = 0; i < Global.howManyEntities; i++)
            {
                if (i == Global.howManyPlayers - 1)
                   entities[i] = new Entity(true, Color.White);
                else 
                    entities[i] = new Entity(false, randomchyk.RandColor());
            }

            while (Global.win.IsOpen)
            {
                GameLoop();
            }
        }

        void CreateFile()
        {
            string path = @"D:\Github\week44\Agario\Agario\bin\Debug\ReadIt.ini";
            string[] lines = { "1920", "1080", "Deez" };

            try
            {
                StreamReader sr = new StreamReader(path);
                sr.Close();
            }
            catch
            {
                var ini = File.Create("ReadIt.ini");
                ini.Close();

                StreamWriter sw = new StreamWriter(path);

                sw.WriteLine(lines[0]);
                sw.WriteLine(lines[1]);
                sw.WriteLine(lines[2]);
                sw.Close();
            }
        }

        void GameLoop()
        {
            Global.win.DispatchEvents();
            Global.win.Clear(new Color(0, 30, 30));

            Update();

            Global.win.Display();
        }

        void Update()
        {
            foreach (var foods in food)
                foods.Update();

            foreach (var entity in entities)
                entity.Update(food, entities);
        }
    }
}