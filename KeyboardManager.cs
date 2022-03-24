using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Sokoban
{
    class KeyboardManager
    {
        private static KeyboardManager instance;
        enum KeyState
        {
            PRESSED,
            HELD,
            UP,
            NONE
        }

        Dictionary<Keys, KeyState> keyAndState;

        public KeyboardManager()
        {
            if (instance == null)
            {
                keyAndState = new Dictionary<Keys, KeyState>();
                instance = this;
            }
            else throw new Exception("Instance already created!");
        }

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressedKeys = state.GetPressedKeys();
            foreach (Keys k in pressedKeys)
            {
                if (!keyAndState.ContainsKey(k))
                {
                    keyAndState.Add(k, KeyState.PRESSED);

                }
                else
                {
                    if (keyAndState[k] == KeyState.PRESSED)
                    {
                        keyAndState[k] = KeyState.HELD;
                    }
                    else if (keyAndState[k] == KeyState.UP || keyAndState[k] == KeyState.NONE) {
                        keyAndState[k] = KeyState.PRESSED;
                    }
                }
            }
            foreach(Keys k in keyAndState.Keys.ToArray())
            {
                if (!pressedKeys.Contains(k))
                {
                    if(keyAndState[k] == KeyState.UP)
                    {
                        keyAndState[k] = KeyState.NONE;
                    } else if(keyAndState[k] == KeyState.PRESSED || keyAndState[k] == KeyState.HELD)
                    {
                        keyAndState[k] = KeyState.UP;
                    }
                }
            }
        }

        public bool IsKeyPressed(Keys k) => keyAndState.ContainsKey(k) && keyAndState[k] == KeyState.PRESSED;
        public bool IsKeyUp(Keys k) => keyAndState.ContainsKey(k) && keyAndState[k] == KeyState.UP;
        public bool IsKeyHeld(Keys k) => keyAndState.ContainsKey(k) && keyAndState[k] == KeyState.HELD;
    }
}
