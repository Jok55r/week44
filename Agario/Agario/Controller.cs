using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Agario
{
    internal class Controller
    {
        public List<Keyboard.Key> keys = new List<Keyboard.Key>();
        public List<bool> isPressed = new List<bool>();

        public Controller(List<Keyboard.Key> keys)
        {
            this.keys = keys;
        }

        public bool Pressed(Keyboard.Key key)
            => Keyboard.IsKeyPressed(key);
    }
}