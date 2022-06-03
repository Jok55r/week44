using SFML.Window;
using SFML.Graphics;
using System;
using SFML.System;

namespace Agario
{
    internal class Game
    {
        const uint fps = 240;
        const int howManyBots = 10;

        Random rnd = new Random();
        Randomchyk randomchyk = new Randomchyk();
        Entity player = new Entity(new Vector2f(0, 0), true, Color.White, 20);
        Entity[] bots = new Entity[howManyBots];
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
            for (int i = 0; i < howManyBots; i++)
            {
                bots[i] = new Entity(randomchyk.RandVect(win), false, randomchyk.RandColor(), rnd.Next(15, 40));
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
            for (int i = 0; i < howManyBots; i++)
            {
                bots[i].Update(food, win, bots);
            }
            for (int i = 0; i < player.playerList.Count; i++)
            {
                player.playerList[i].Update(food, win, bots);
            }
        }

        public void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}