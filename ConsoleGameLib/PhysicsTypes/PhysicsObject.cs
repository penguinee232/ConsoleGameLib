using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGameLib;
using System.Text;
using ConsoleGameLib.CoreTypes;
using ConsoleGameLib.Helpers;

namespace ConsoleGameLib.PhysicsTypes
{

    

    /// <summary>
    /// Work in progress. Will allow for many points treated as one.
    /// </summary>
    public class PhysicsObject
    {


        public event EventHandler Contact;
        


        public List<ObjectPoint> ContainedPoints
        {
            get
            {
                return Contents;
            }
            set
            {
                Contents = value;
                foreach (ObjectPoint point in Contents)
                {
                    point.Object = this;
                }
            }
        }

        public Point Position = new Point();


        List<ObjectPoint> Contents = new List<ObjectPoint>();




        public Point Velocity;


        public int Mass;



        public Point Momentum
        {
            get { return Velocity * Mass; }
        }


        private List<ObjectPoint> exteriorPoints = new List<ObjectPoint>();

        public PhysicsWorld World;



        public bool ObeysGravity = false;

        /// <summary>
        /// True if physics should be calculated for the whole object. False if each point in the object should be calculated individually when it comes to physics.
        /// </summary>
        public bool Unified = true;

        public bool InteractsWithEnvironment = true;

        public PhysicsObject(bool obeysGravity, bool obeysDrag, bool interactsWithEnvironment)
        {

        }

        public void Update()
        {
            if (Unified)
            {
                Point bottomLeft = Contents.BottomLeft();
                Point topRight = Contents.TopRight();
                exteriorPoints = (List<ObjectPoint>)Contents.ExteriorPoints();
                ProcessVelocity();
                foreach(ObjectPoint point in Contents)
                {
                    point.Position = point.RelativePosition + Position;
                } 







            }
            else
            {
                //foreach (PhysicsPoint point in Contents)
                //{
                //    point.Update();
                //}
            }
        }

        public void Draw()
        {
            foreach (ObjectPoint point in Contents)
            {
                point.Draw();
            }
        }

        bool Colliding()
        {
            
            return CollidingLeft() || CollidingRight() || CollidingTop() || CollidingBottom();
        }

        bool CollidingBottom()
        {
            foreach (ObjectPoint point in exteriorPoints)
            {
                if (InteractsWithEnvironment && (World.Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y - 1)) && !Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y - 1))))
                {
                    return true;
                }

            }
            return false;
        }
        bool CollidingLeft()
        {
            foreach (ObjectPoint point in exteriorPoints)
            {
                if (InteractsWithEnvironment && (World.Contents.ContainsPoint(new Point(point.Position.X - 1, point.Position.Y)) && !Contents.ContainsPoint(new Point(point.Position.X - 1, point.Position.Y))))
                {
                    return true;
                }
                

            }
            return false;
        }
        bool CollidingRight()
        {
            foreach (ObjectPoint point in exteriorPoints)
            {
                if (InteractsWithEnvironment && (World.Contents.ContainsPoint(new Point(point.Position.X + 1, point.Position.Y)) && !Contents.ContainsPoint(new Point(point.Position.X + 1, point.Position.Y))))
                {
                    return true;
                }

            }
            return false;
        }
        bool CollidingTop()
        {
            foreach (ObjectPoint point in exteriorPoints)
            {
                if (InteractsWithEnvironment && (World.Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y + 1)) && !Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y + 1))))
                {
                    return true;
                }

            }
            return false;
        }



        void ProcessVelocity()
        {
            Point tempVel = Velocity;
            while (tempVel.X != 0 || tempVel.Y != 0)
            {
                if (tempVel.Y > 0)
                {
                    if (!CollidingTop() || !InteractsWithEnvironment)
                    {
                        Position.Y++;
                        tempVel.Y--;
                    }
                    else if (InteractsWithEnvironment)
                    {
                        Contact?.Invoke(this, EventArgs.Empty);
                        tempVel.Y = 0;
                    }
                }
                else if (tempVel.Y < 0)
                {
                    if (CollidingBottom() && InteractsWithEnvironment)
                    {
                        Contact?.Invoke(this, EventArgs.Empty);
                        tempVel.Y = 0;

                    }
                    else
                    {
                        Position.Y--;
                        tempVel.Y++;
                    }
                }

                if (tempVel.X > 0)
                {
                    if (!CollidingRight() || !InteractsWithEnvironment)
                    {
                        Position.X++;
                        tempVel.X--;
                    }
                    else if (InteractsWithEnvironment)
                    {
                        Contact?.Invoke(this, EventArgs.Empty);
                        tempVel.X = 0;
                    }
                }
                else if (tempVel.X < 0)
                {
                    if (CollidingLeft() && InteractsWithEnvironment)
                    {
                        Contact?.Invoke(this, EventArgs.Empty);
                        tempVel.X = 0;

                    }
                    else
                    {
                        Position.X--;
                        tempVel.X++;
                    }
                }
            }
            //if (ObeysDrag && World.CurrentDragUpdateInCycle >= World.LinearDragCalculationInterval)
            //{
            //    int velX = Velocity.X;
            //    if (velX != 0)
            //    {
            //        Velocity.X -= Velocity.X > 0 ? World.LinearDrag : -1 * World.LinearDrag;
            //    }
            //    if (velX > 0 && Velocity.X < 0)
            //    {
            //        Velocity.X = (int)MathHelper.ClampMin(0, Velocity.X);
            //    }
            //    else if (velX < 0 && Velocity.X > 0)
            //    {
            //        Velocity.X = (int)MathHelper.ClampMax(0, Velocity.X);
            //    }

            //}
            //if (ObeysGravity && World.CurrentGravityUpdateInCycle >= World.GravityCalculationInterval)
            //{
            //    Velocity.Y -= Velocity.Y > World.TerminalFallVelocity ? World.GravitationalAcceleration : 0;
            //    Velocity.Y = (int)MathHelper.ClampMin(World.TerminalFallVelocity, Velocity.Y);
            //}
            //if (World.Contents.ContainsPoint(new Point(Position.X, Position.Y - 1)) && InteractsWithEnvironment)
            //{
            //    Velocity.Y = 0;
            //}
        }
    }
}