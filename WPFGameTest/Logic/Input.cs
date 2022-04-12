using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFGameTest.Logic
{
    public static class Input
    {
        public static Key pressedKey = Key.None;
        public static Key releasedKey = Key.None;
        public static bool[] heldKeys = new bool[(int)Key.DeadCharProcessed];

        public static bool GetKey(Key key)
        {
            return heldKeys[(int)key];
        }

        public static bool GetKeyPressed(Key key)
        {
            if (key == pressedKey && pressedKey != Key.None)
            {
                pressedKey = Key.None;
                return true;
            }

            return false;
        }

        public static bool GetKeyReleased(Key key)
        {
            if (key == releasedKey && releasedKey != Key.None)
            {
                releasedKey = Key.None;
                return true;
            }

            return false;
        }

        public static bool GetMouseButton(MouseButtonState mouseButton)
        {
            return mouseButton == MouseButtonState.Pressed;
        }
    }
}
