using ConsoleGameLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGameLib.PhysicsTypes
{
    /// <summary>
    /// For internal use in the PhysicsObject class.
    /// </summary>
    public class ObjectPoint
    {
        public Point Position;
        public ConsoleColor Color;


        public Point RelativePosition;
        public PhysicsObject Object;

        



        public void Draw()
        {
            Console.ForegroundColor = Color;
            if (Position.Y > 0 && Position.X < Object.World.ScreenSize.Width)
            {
                Console.SetCursorPosition(Position.X, Object.World.ScreenSize.Height - Position.Y);
                Console.Write('█');
            }
        }
    }
}
