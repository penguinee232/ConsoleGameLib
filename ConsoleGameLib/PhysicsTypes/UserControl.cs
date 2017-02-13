using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGameLib.PhysicsTypes
{
    public class UserControl
    {
        public bool ClearBuffer = true;

        public event EventHandler<KeyPressArgs> KeyPress;

        public void Update()
        {
            if(Console.KeyAvailable)
            {
                KeyPress?.Invoke(this, new KeyPressArgs(Console.ReadKey(true)));
            }
            while(ClearBuffer && Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }


        
    }
}
