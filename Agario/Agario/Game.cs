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

        readonly private Randomchyk randomchyk = new Randomchyk();
        readonly private Entity[] entities = new Entity[howManyEntities];
        readonly private Food[] food = new Food[Food.howManyFood];

        public void GameStart()
        {
            Global.win.Closed += Global.WindowClosed;
            Global.win.SetFramerateLimit(fps);

            for (int i = 0; i < Food.howManyFood; i++)
            {
                food[i] = new Food(randomchyk.RandVect(), randomchyk.RandColor());
            }
            for (int i = 0; i < howManyEntities; i++)
            {
                if (i == Global.howManyPlayers)
                   entities[i] = new Entity(randomchyk.RandVect(), true, Color.White, randomchyk.RandNum(15, 40));
                else 
                    entities[i] = new Entity(randomchyk.RandVect(), false, randomchyk.RandColor(), randomchyk.RandNum(15, 40));
            }

            while (Global.win.IsOpen)
            {
                GameLoop();
            }
        }

        void GameLoop()
        {
            Global.win.DispatchEvents();
            Global.win.Clear(new Color(0, 30, 30));

            Update();

            Global.win.Display();
        }

        void Update()
        {
            foreach (var foods in food)
                foods.Update();

            foreach (var entity in entities)
                entity.Update(food, entities);
        }
    }
}