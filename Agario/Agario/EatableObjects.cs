using System.Collections.Generic;

namespace Agario
{
    internal class EatableObjects
    {
        public List<Ball> eatable = new List<Ball>();

        public EatableObjects(Ball[] entities, Ball[] food)
        {
            for (int i = 0; i < entities.Length + food.Length; i++)
            {
                if (i < entities.Length)
                    eatable.Add(entities[i]);
                else
                    eatable.Add(food[i - entities.Length]);
            }
        }
    }
}