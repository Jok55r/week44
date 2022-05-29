using SFML.Window;
using SFML.Graphics;
using System;
using System.Threading;

namespace Agario
{
    internal class Game
    {
        const uint width = 1920;
        const uint height = 1080;
        const uint fps = 240;

        public void GameLoop()
        {
            RenderWindow window = new RenderWindow(new VideoMode(width, height), "Game window");
            window.Closed += WindowClosed;
            window.SetFramerateLimit(fps);

            Player player = new Player();
            Food food = new Food(window);

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear(new Color(0, 30, 30));
                window.Display();

                food.Update(window);
                player.Update(window);
            }
        }

        public void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}