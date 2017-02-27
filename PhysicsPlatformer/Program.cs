﻿using System;
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
       static PhysicsObject userObj;

        static PhysicsWorld world;

        static Random rand;

        static int jumpVel = 3;

        static PhysicsObject obstacles;
        static void Main(string[] args)
        {
            //Create objects and make cursor invisible
            world = new PhysicsWorld();
            List<PhysicsObject> objects = new List<PhysicsObject>();

            Console.CursorVisible = false;


            userObj = new PhysicsObject(true, true, true);
            userObj.ContainedPoints.Add(new ObjectPoint(ConsoleColor.Blue, new Point(0,0),userObj));
            userObj.Position = new Point(0, 36);
            userObj.Name = "Player";

            userObj.ObeysGravity = true;

            objects.Add(userObj);
            //Set world parameters.

            //Perform gravity calculations every 1 update.
            world.GravityCalculationInterval = 2;


            //Every GravityCalculationInterval updates, remove 1 from each gravity-obeying point's y velocity. 
            world.GravitationalAcceleration = 1;

            world.TerminalFallVelocity = -3;

            world.Drag = 1;

            //Perform drag calculations every 1 update.
            world.DragCalculationInterval = 1;

            //Define the point that the user controls. This one obeys gravity, interacts with the environment, and is blue.
            UserControl control = new UserControl();
            world.Control = control;
            control.KeyPress += Control_KeyPress;

            rand = new Random();

            //Create a platform on which the player starts

            List<ObjectPoint> points = new List<ObjectPoint>();
            obstacles = new PhysicsObject(false, false, true);
            for (int i = 0; i < 15; i++)
            {
                points.Add(new ObjectPoint(ConsoleColor.Green, new Point(i,35),obstacles));
            }


            //Create a placeholder, manipulatable location
            Point tempPos = new Point(15, 35);


            for (int j = 0; j < 10; j++)
            {
                //Generate 10 platforms of random length, defined by moving around tempPos
                int length = rand.Next(1, 10);

                tempPos.Y += rand.Next(0, 2);
                tempPos.X += rand.Next(1, 3);

                for (int i = 0; i < length; i++)
                {
                    points.Add(new ObjectPoint(ConsoleColor.Green, tempPos, obstacles));
                    tempPos.X++;
                }
            }

            obstacles.Position = new Point(0, 0);
            obstacles.ContainedPoints = points;

            objects.Add(obstacles);

            //Set world points to the points we just defined
            
            world.Objects = objects;

            //Perform operations forever
            while (true)
            {


                //Update and draw the world
                world.Update();

                world.Draw();

                //If the player has fallen, reset their position
                if (userObj.Position.Y <= 10)
                {
                    userObj.Position = new Point(0, 36);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, world.ScreenSize.Height - 45);
                Console.Write($"Effective Gravity: {(float)world.GravitationalAcceleration/world.GravityCalculationInterval}\nEffective Drag: {(float)world.Drag/world.DragCalculationInterval}\nTerminal velocity: {world.TerminalFallVelocity}\nJumping velocity: {jumpVel}");

                Console.SetCursorPosition(40, world.ScreenSize.Height - 45);
                Console.Write($"Velocity: {{{userObj.Velocity.X},{userObj.Velocity.Y}}}");
                Console.SetCursorPosition(40, world.ScreenSize.Height - 44);
                Console.Write($"Position: {{{userObj.Position.X},{userObj.Position.Y}}}");
                //Wait 50 ms before the next update
                Thread.Sleep(50);




            }

        }

        private static void Control_KeyPress(object sender, KeyPressArgs e)
        {
            if (e.Key.KeyChar == ' ' && world.Objects.ContainsPoint(new Point(userObj.Position.X,userObj.Position.Y - 1)))
            {
                userObj.Velocity.Y += jumpVel;
            }
            if (e.Key.KeyChar == 'a')
            {
                userObj.Velocity.X += -1;
            }
            if (e.Key.KeyChar == 'd')
            {
                userObj.Velocity.X += 1;
            }
            if(e.Key.KeyChar == 'r')
            {
                #region reset
                List<ObjectPoint> points = new List<ObjectPoint>();
                for (int i = 0; i < 15; i++)
                {
                    points.Add(new ObjectPoint(ConsoleColor.Green, new Point(i, 35), obstacles));
                }
                //Create a placeholder, manipulatable location
                Point tempPos = new Point(15, 35);
                

                

                for (int j = 0; j < 10; j++)
                {
                    //Generate 10 platforms of random length, defined by moving around tempPos
                    int length = rand.Next(1, 10);

                    tempPos.Y += rand.Next(0, 2);
                    tempPos.X += rand.Next(1, 3);

                    for (int i = 0; i < length; i++)
                    {
                        points.Add(new ObjectPoint(ConsoleColor.Green, tempPos, obstacles));
                        tempPos.X++;
                    }
                }

                obstacles.Position = new Point(0, 0);
                obstacles.ContainedPoints = points;
                #endregion
                userObj.Position.Y = 37;
            }
            if(e.Key.KeyChar == 'c')
            {
                world.DragCalculationInterval = rand.Next(1,7);
                world.GravityCalculationInterval = rand.Next(1, 7);

                world.TerminalFallVelocity = rand.Next(-10, -2);

                jumpVel = rand.Next(2, 6);
            }
        }

        
    }
}
