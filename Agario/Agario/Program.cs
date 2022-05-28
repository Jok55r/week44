using SFML.Window;
using SFML.Graphics;
using System;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(1600, 900), "Game window");
            window.Closed += WindowClosed;

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear(Color.Green);

                window.Display();
            }
        }

        static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}
