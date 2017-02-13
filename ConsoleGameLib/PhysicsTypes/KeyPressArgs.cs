using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGameLib.PhysicsTypes
{
    public class KeyPressArgs : EventArgs
    {
        public ConsoleKeyInfo Key;

        public KeyPressArgs(ConsoleKeyInfo key)
        {
            Key = key;
        }
    }
}
