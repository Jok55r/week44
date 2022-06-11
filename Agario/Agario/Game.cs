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

        Randomchyk randomchyk = new Randomchyk();
        Entity[] entities = new Entity[howManyEntities];
        Food[] food; 
        Food f;
        RenderWindow win = new RenderWindow(new VideoMode(1920, 1080), "Game window");

        public void GameStart()
        {
            win.Closed += WindowClosed;
            win.SetFramerateLimit(fps);

            f = new Food(randomchyk.RandVect(win.Size), randomchyk.RandColor());
            food = new Food[f.howManyFood];

            for (int i = 0; i < f.howManyFood; i++)
            {
                food[i] = new Food(randomchyk.RandVect(win.Size), randomchyk.RandColor());
            }
            for (int i = 0; i < howManyEntities; i++)
            {
                if (i == 0)
                   entities[i] = new Entity(randomchyk.RandVect(win.Size), true, Color.White, randomchyk.RandNum(15, 40));
                else 
                    entities[i] = new Entity(randomchyk.RandVect(win.Size), false, randomchyk.RandColor(), randomchyk.RandNum(15, 40));
            }

            while (win.IsOpen)
            {
                GameLoop();
            }
        }

        void GameLoop()
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

        /*void Loops(int howMany, Object[] obj)
        {
            for(int i = 0; i < howMany; i++)
            {
                obj[i].Update();
            }
        }*/

        public void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}