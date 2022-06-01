﻿using SFML.Window;
using SFML.Graphics;
using System;
using SFML.System;

namespace Agario
{
    internal class Game
    {
        Random rnd = new Random();
        const uint width = 1920;
        const uint height = 1080;
        const uint fps = 240;
        public const int howManyFood = 2500;

        public void GameLoop()
        {
            RenderWindow win = new RenderWindow(new VideoMode(width, height), "Game window");
            win.Closed += WindowClosed;
            win.SetFramerateLimit(fps);

            Player player = new Player(new Vector2f(0, 0));
            Food[] food = new Food[howManyFood];
            for (int i = 0; i < howManyFood; i++) {
                food[i] = new Food(win, new Vector2f(rnd.Next(0, (int)win.Size.X), rnd.Next(0, (int)win.Size.Y)),
                    new Color((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256)));
            }

            while (win.IsOpen)
            {
                win.DispatchEvents();
                win.Clear(new Color(0, 30, 30));

                for (int i = 0; i < howManyFood; i++) {
                    win.Draw(food[i].foodObj);
                }
                for (int i = 0; i < player.players.Count; i++) {
                    win.Draw(player.players[i].playerObj);
                }
                //win.Draw(player.playerObj);
                win.Display();

                for (int i = 0; i < player.players.Count; i++) {
                    player.players[i].Update(food, win);
                }
            }
        }

        public void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}