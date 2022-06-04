using SFML.Window;
using SFML.Graphics;
using System;
using SFML.System;

namespace Agario
{
    internal class Game
    {
        const uint fps = 240;
        const int howManyEntities = 10;

        Random rnd = new Random();
        Randomchyk randomchyk = new Randomchyk();
        Entity[] entities = new Entity[howManyEntities];
        Food[] food; 
        Food f;
        RenderWindow win = new RenderWindow(new VideoMode(1920, 1080), "Game window");

        public void GameLoop()
        {
            win.Closed += WindowClosed;
            win.SetFramerateLimit(fps);

            f = new Food(randomchyk.RandVect(win), randomchyk.RandColor());
            food = new Food[f.howManyFood];

            for (int i = 0; i < f.howManyFood; i++)
            {
                food[i] = new Food(randomchyk.RandVect(win), randomchyk.RandColor());
            }
            for (int i = 0; i < howManyEntities; i++)
            {
                if (i == 0)
                   entities[i] = new Entity(randomchyk.RandVect(win), true, Color.White, rnd.Next(15, 40));
                else 
                    entities[i] = new Entity(randomchyk.RandVect(win), false, randomchyk.RandColor(), rnd.Next(15, 40));
            }

            while (win.IsOpen)
            {
                GameLogic();
            }
        }

        void GameLogic()
        {
            win.DispatchEvents();
            win.Clear(new Color(0, 30, 30));

            Update();

            win.Display();
        }

        void Update()
        {
            for (int i = 0; i < f.howManyFood; i++)
            {
                food[i].Update(win);
            }
            for (int i = 0; i < howManyEntities; i++)
            {
                entities[i].Update(food, win, entities);
            }
        }

        public void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}