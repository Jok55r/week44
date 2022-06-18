using SFML.Graphics;
using System;

namespace Agario
{
    internal class Game
    {
        readonly private Randomchyk randomchyk = new Randomchyk();
        readonly private Entity[] entities = new Entity[Global.howManyEntities];
        readonly private Food[] food = new Food[Food.howManyFood];

        public void GameStart()
        {
            Global.win.Closed += Global.WindowClosed;
            Global.win.SetFramerateLimit(Global.fps);

            for (int i = 0; i < Food.howManyFood; i++)
            {
                food[i] = new Food(randomchyk.RandVect(), randomchyk.RandColor());
            }

            for (int i = 0; i < Global.howManyEntities; i++)
            {
                if (i == Global.howManyPlayers - 1)
                   entities[i] = new Entity(true, Color.White);
                else 
                    entities[i] = new Entity(false, randomchyk.RandColor());
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