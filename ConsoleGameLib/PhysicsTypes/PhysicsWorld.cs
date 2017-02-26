using ConsoleGameLib.CoreTypes;
using ConsoleGameLib.PhysicsTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameLib
{
    public class PhysicsWorld
    {
        private List<PhysicsPoint> points;

       

        private List<PhysicsObject> objects = new List<PhysicsObject>();

        public List<PhysicsObject> Objects
        {
            get { return objects; }
            set { objects = value; }
        }




        public List<PhysicsPoint> Contents
        {
            get { return points; }
            set { points = value; }

        }

        public UserControlledPoint UserPoint = null;

        public UserControl Control = null;



        

        private Size screenSize;

        /// <summary>
        /// Updates between gravity calculations. 1 or below means calculate every update.
        /// </summary>
        public int GravityCalculationInterval = 1;

        /// <summary>
        /// Every time drag is calculated, each PhysicsObject has its velocity divided by this value.
        /// </summary>
        public int Drag = 1;

        /// <summary>
        /// Updates between drag calculations. 1 or below means calculate every update.
        /// </summary>
        public int DragCalculationInterval = 1;


        int currentGravUpdate = 0;

        int currentDragUpdate = 0;

        public int CurrentGravityUpdateInCycle
        {
            get { return currentGravUpdate; }
        }

        public int CurrentDragUpdateInCycle
        {
            get { return currentDragUpdate; }
        }


        public Size ScreenSize
        {
            get
            {
                return screenSize;
            }
            set
            {
                screenSize = value;
                Console.SetBufferSize(ScreenSize.Width, ScreenSize.Height);
            }
        }



        public int TerminalFallVelocity = -3;

        public int GravitationalAcceleration = 1;

        public PhysicsWorld()
            :this(new Size(Console.BufferWidth, Console.BufferHeight))
        {
            
        }

        public PhysicsWorld(Size screenSize)
        {
            ScreenSize = screenSize;
        }

        public void Update()
        {
            currentGravUpdate++;
            currentDragUpdate++;
            if (objects != null)
            {
                foreach (PhysicsObject obj in objects)
                {
                    obj.World = this;
                    obj.Update();
                }
            }
            
            Control?.Update();

            if(currentGravUpdate >= GravityCalculationInterval)
            {
                currentGravUpdate = 0;
            }
            if (currentDragUpdate >= DragCalculationInterval)
            {
                currentDragUpdate = 0;
            }

        }

        public void Draw()
        {
            Console.Clear();
            if (objects != null)
            {
                foreach (PhysicsObject obj in objects)
                {
                    if (obj.Position.X >= 0 && obj.Position.X <= ScreenSize.Width && obj.Position.Y >= 0 && obj.Position.Y <= ScreenSize.Height)
                    {
                        obj.Draw();
                    }
                }
            } 
        }

        
    }
}
