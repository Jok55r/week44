using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Agario
{
    internal class UI
    {
        private readonly List<Text> text = new List<Text>();

        public UI(Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                text.Add(new Text("", new Font(@"D:\Github\week44\Agario\Agario\shrift.ttf"))
                {
                    Position = new Vector2f(1800, (i + 1) * 50),
                    Scale = new Vector2f(1, 1)
                });
            }
        }

        void UpdateStats()
        {
            for (int i = 0; i < text.Count; i++)
                text[i].DisplayedString = $"-{i}- hidkgf";
        }

        public void Update()
        {
            UpdateStats();
            for (int i = 0; i < text.Count; i++)
                Global.win.Draw(text[i]);
        }
    }
}