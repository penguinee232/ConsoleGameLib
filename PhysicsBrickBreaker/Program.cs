using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGameLib.CoreTypes;
using ConsoleGameLib.PhysicsTypes;
using ConsoleGameLib;
using System.Threading;

namespace PhysicsBrickBreaker
{
    class Program
    {
        static void Main(string[] args)
        {
            PhysicsWorld world = new PhysicsWorld();
            world.ClearKeyBufferInUpdate = false;
            world.LinearDrag = 0;
            world.GravitationalAcceleration = 0;
            UserControlledPoint user = new UserControlledPoint(false, new Point(50, 50), true, ConsoleColor.Blue, world);

            

            world.UserPoint = user;

            List<PhysicsPoint> asteroids = new List<PhysicsPoint>();
            List<PhysicsPoint> lasers = new List<PhysicsPoint>();

            world.Contents = lasers;

            while (true)
            {
                world.Update();
                world.Draw();

                if(Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if((int)key >= (int)ConsoleKey.NumPad0 && (int)key <= (int)ConsoleKey.NumPad9)
                    {
                        int val = (int)key - (int)ConsoleKey.NumPad0;
                       // Point pos = new Point(val % 4);
                        lasers.Add(new PhysicsPoint(false, user.Position + new Point(-1, -1), true, ConsoleColor.Red, world));
                        lasers[lasers.Count - 1].Velocity = new Point(-1, -1);
                    }
                   
                }


                Console.SetCursorPosition(user.Position.X, world.ScreenSize.Height - user.Position.Y);

                Thread.Sleep(50);
            }
        }
    }
}
