using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace Agario
{
    internal class UI
    {
        private readonly List<Text> text = new List<Text>();

        public void Create(Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                text.Add(new Text("", new Font(@"D:\Github\week44\Agario\Agario\font.ttf"))
                {
                    Position = new Vector2f(10, i * 50),
                    Scale = new Vector2f(1, 1)
                });
            }
        }

        void UpdateStats(Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].isPlayer)
                    text[i].DisplayedString = $"{entities[i].score} (Player)";
                else
                    text[i].DisplayedString = $"{entities[i].score} (Bot)";
            }
        }

        public void Update(Entity[] entities)
        {
            UpdateStats(entities);
            for (int i = 0; i < text.Count; i++)
                Global.win.Draw(text[i]);
        }
    }
}