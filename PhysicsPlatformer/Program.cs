using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGameLib.CoreTypes;
using ConsoleGameLib.PhysicsTypes;
using ConsoleGameLib;
using System.Threading;


namespace PhysicsPlatformer
{
    class Program
    {

        static void Main(string[] args)
        {
            //Create objects and make cursor invisible
            PhysicsWorld world = new PhysicsWorld();
            List<PhysicsPoint> points = new List<PhysicsPoint>();

            Console.CursorVisible = false;


            //Set world parameters.

            //Perform gravity calculations every 1 update.
            world.GravityCalculationInterval = 1;


            //Every GravityCalculationInterval updates, remove 1 from each gravity-obeying point's y velocity. 
            world.GravitationalAcceleration = 1;
            


            //Perform x-velocity drag calculations every 1 update.
            world.LinearDragCalculationInterval = 1;

            //Define the point that the user controls. This one obeys gravity, interacts with the environment, and is blue.
            UserControlledPoint userPoint = new UserControlledPoint(true,new Point(0,36),true,ConsoleColor.Blue,world);

            //Pressing buttons doesn't automatically move the point. We'll do our own calculations.
            userPoint.UpDistance = 0;
            userPoint.RightDistance = 0;
            userPoint.LeftDistance = 0;

            //Define the functions that trigger when we press the movement keys.
            userPoint.OnLegalRight += UserPoint_OnLegalRight;

            userPoint.OnLegalLeft += UserPoint_OnLegalLeft;


            userPoint.OnLegalUp += UserPoint_OnLegalUp;

            

            //Set the move-up key to be spacebar.
            userPoint.MoveUpKey = ConsoleKey.Spacebar;

            //The user can't move down.
            userPoint.CanMoveDown = false;

            Random rand = new Random();

            //Create a platform on which the player starts
            for (int i = 0; i < 15; i++)
            {
                points.Add(new PhysicsPoint(false,new Point(i,35),true,ConsoleColor.Green,world));
            }


            //Create a placeholder, manipulatable location
            Point tempPos = new Point(15,35);

            
            for (int j = 0; j < 10; j++)
            {
                //Generate 10 platforms of random length, defined by moving around tempPos
                int length = rand.Next(1, 10);

                tempPos.Y += rand.Next(0, 2);
                tempPos.X += rand.Next(1, 3);

                for (int i = 0; i < length; i++)
                {
                    points.Add(new PhysicsPoint(false, new Point(tempPos.X, tempPos.Y), true, ConsoleColor.Green, world));
                    tempPos.X++;
                }
            }
            
            //Set world points to the points we just defined
            world.UserPoint = userPoint;
            world.Contents = points;

            //Perform operations forever
            while(true)
            {
                
                
                //Update and draw the world
                world.Update();

                world.Draw();

                //If the player has fallen, reset their position
                if (userPoint.Position.Y <= 10)
                {
                    userPoint.Position = new Point(0,36);
                }


                //Wait 50 ms before the next update
                Thread.Sleep(50);

                


            }

        }

        private static void UserPoint_OnLegalLeft(object sender, EventArgs e)
        {
            //When pressing the left movement key, accelerate the point to the left.
            ((UserControlledPoint)sender).Velocity.X-= 1;
        }

        private static void UserPoint_OnLegalRight(object sender, EventArgs e)
        {
            //When pressing the right movement key, accelerate the point to the right.
            ((UserControlledPoint)sender).Velocity.X+= 1;
        }

        private static void UserPoint_OnLegalUp(object sender, EventArgs e)
        {
            //When pressing the up key and the point is resting on an object, hurl the point into the air.
            UserControlledPoint sendPoint = (UserControlledPoint)sender;
            if (sendPoint.World.Contents.ContainsPoint(new Point(sendPoint.Position.X, sendPoint.Position.Y - 1)) && sendPoint.Velocity.Y == 0)
            {
                sendPoint.Velocity.Y += 3;
            }
        }
    }
}
