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

        public string Name { get; set; } = "";
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



        public bool InteractsWithEnvironment = true;

        public PhysicsObject(bool obeysGravity, bool obeysDrag, bool interactsWithEnvironment)
        {
            ObeysGravity = obeysGravity;
            InteractsWithEnvironment = interactsWithEnvironment;
        }

        public void Update()
        {

            foreach (ObjectPoint point in Contents)
            {
                point.Position = point.RelativePosition + Position;
            }
            Point bottomLeft = Contents.BottomLeft();
            Point topRight = Contents.TopRight();

            exteriorPoints = (List<ObjectPoint>)Contents.ExteriorPoints();
            ProcessVelocity();
            foreach (ObjectPoint point in Contents)
            {
                point.Position = point.RelativePosition + Position;
            }

        }

        public void Draw()
        {
            foreach (ObjectPoint point in Contents)
            {
                point.Draw();
            }
        }

        bool Colliding
        {

            get { return CollidingLeft || CollidingRight || CollidingTop || CollidingBottom; }
        }

        bool CollidingBottom
        {
            get
            {
                foreach (ObjectPoint point in exteriorPoints)
                {
                    if (InteractsWithEnvironment && (World.Objects.ContainsPoint(new Point(point.Position.X, point.Position.Y - 1)) && !Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y - 1))))
                    {
                         return true;
                    }

                }
                return false;
            }
        }
        bool CollidingLeft
        {
            get
            {
                foreach (ObjectPoint point in exteriorPoints)
                {
                    if (InteractsWithEnvironment && (World.Objects.ContainsPoint(new Point(point.Position.X - 1, point.Position.Y)) && !Contents.ContainsPoint(new Point(point.Position.X - 1, point.Position.Y))))
                    {
                        return true;
                    }


                }
                return false;
            }
        }
        bool CollidingRight
        {
            get
            {
                foreach (ObjectPoint point in exteriorPoints)
                {
                    if (InteractsWithEnvironment && (World.Objects.ContainsPoint(new Point(point.Position.X + 1, point.Position.Y)) && !Contents.ContainsPoint(new Point(point.Position.X + 1, point.Position.Y))))
                    {
                        return true;
                    }

                }
                return false;
            }
        }
        bool CollidingTop
        {
            get
            {
                foreach (ObjectPoint point in exteriorPoints)
                {
                    if (InteractsWithEnvironment && (World.Objects.ContainsPoint(new Point(point.Position.X, point.Position.Y + 1)) && !Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y + 1))))
                    {
                        return true;
                    }

                }
                return false;
            }
        }



        void ProcessVelocity()
        {
            Point tempVel = Velocity;
            while (tempVel.X != 0 || tempVel.Y != 0)
            {
                foreach (ObjectPoint point in Contents)
                {
                    point.Position = point.RelativePosition + Position;
                }
                if (tempVel.Y > 0)
                {
                    if (!CollidingTop || !InteractsWithEnvironment)
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

                    //if (Position.Y <= 36 && Name == "Player")
                    //{

                    //}

                    if (CollidingBottom && InteractsWithEnvironment)
                    {
                        Contact?.Invoke(this, EventArgs.Empty);
                        tempVel.Y = 0;
                        Velocity.Y = 0;////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        Position.Y--;
                        tempVel.Y++;
                    }
                }

                if (tempVel.X > 0)
                {
                    if (!CollidingRight || !InteractsWithEnvironment)
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
                    if (CollidingLeft && InteractsWithEnvironment)
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
            if (ObeysGravity && World.CurrentGravityUpdateInCycle >= World.GravityCalculationInterval)
            {
                Velocity.Y -= Velocity.Y > World.TerminalFallVelocity ? World.GravitationalAcceleration : 0;
                Velocity.Y = (int)MathHelper.ClampMin(World.TerminalFallVelocity, Velocity.Y);
            }

            if (World.CurrentDragUpdateInCycle >= World.DragCalculationInterval)
            {
                if (Velocity.X > 0)
                {
                    Velocity.X -= World.Drag;
                    Velocity.X = (int)MathHelper.ClampMin(0, Velocity.X);
                }
                else if (Velocity.X < 0)
                {

                    Velocity.X += World.Drag;
                    Velocity.X = (int)MathHelper.ClampMax(0, Velocity.X);
                }

                //if (Velocity.Y > 0)
                //{
                //    Velocity.Y -= World.Drag;
                //    Velocity.Y = (int)MathHelper.ClampMin(0, Velocity.Y);
                //}
                //else if (Velocity.Y < 0)
                //{

                //    Velocity.Y += World.Drag;
                //    Velocity.Y = (int)MathHelper.ClampMax(0, Velocity.Y);
                //}


                //if (ObeysGravity && World.Drag == World.GravitationalAcceleration && Velocity.Y == 0)
                //{
                //    Velocity.Y -= World.GravitationalAcceleration;
                //}
            }


        }
    }
}