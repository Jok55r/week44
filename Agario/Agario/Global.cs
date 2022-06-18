using SFML.Window;
using SFML.Graphics;
using System.IO;
using System;

namespace Agario
{
    public static class Global
    {
        readonly private static string[] lines = File.ReadAllLines(@"D:\Github\week44\Agario\Agario\bin\Debug\ReadIt.ini");
        public static byte howManyPlayers = 1;
        public const uint fps = 240;
        public const int howManyEntities = 10;

        public static RenderWindow win = new RenderWindow
            (new VideoMode(uint.Parse(lines[0]), uint.Parse(lines[1])), lines[2]);

        public static void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
    }
}