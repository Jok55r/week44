using SFML.Window;
using SFML.Graphics;
using System.IO;
using System;

namespace Agario
{
    public static class Global
    {
        public const int scale = 5;
        readonly private static string[] lines = File.ReadAllLines(@"D:\Github\week44\Agario\Agario\bin\Debug\ReadIt.ini");
        public static byte howManyPlayers = 1;
        public const uint fps = 240;
        public const int howManyEntities = 10;

        public static RenderWindow win = new RenderWindow
            (new VideoMode(Try(lines[0]), Try(lines[1])), lines[2]);

        private static uint Try(string line)
        {
            if (uint.TryParse(line, out var number))
                return uint.Parse(line);

            return 1000;
        }

        public static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}