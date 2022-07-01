using SFML.Graphics;
using System.IO;
using System;
using SFML.Window;

namespace Agario
{
    internal class Game
    {
        readonly private Entity[] entities = new Entity[Global.howManyEntities];
        readonly private Food[] food = new Food[Food.howManyFood];
        private EatableObjects eatableClass;
        readonly private UI ui = new UI();

        public void GameStart()
        {
            CreateFile();

            Global.win.Closed += WindowClosed;
            Global.win.SetFramerateLimit(Global.fps);

            for (int i = 0; i < Food.howManyFood; i++)
                food[i] = new Food();

            for (int i = 0; i < Global.howManyEntities; i++)
            {
                if (i == Global.howManyPlayers - 1)
                    CreatePlayer(i);
                else
                    CreateBot(i);
            }
            eatableClass = new EatableObjects(entities, food);

            ui.Create(entities);

            while (Global.win.IsOpen)
                GameLoop();
        }

        void CreatePlayer(int i)
            => entities[i] = new Entity(true);
        void CreateBot(int i)
            => entities[i] = new Entity(false);

        void CreateFile()
        {
            string path = @"D:\Github\week44\Agario\Agario\bin\Debug\ReadIt.ini";

            if (File.Exists(path))
                return;

            string[] lines = { "1920", "1080", "Deez" };
            var ini = File.Create("ReadIt.ini");
            ini.Close();

            StreamWriter sw = new StreamWriter(path);

            for (int i = 0; i < lines.Length; i++)
                sw.WriteLine(lines[i]);
            sw.Close();
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
                entity.Update(food, entities, eatableClass.eatable);

            ui.Update(entities);
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}