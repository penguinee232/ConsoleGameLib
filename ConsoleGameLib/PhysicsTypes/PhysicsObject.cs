using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGameLib;
using System.Text;
using ConsoleGameLib.CoreTypes;

namespace ConsoleGameLib.PhysicsTypes
{
    /// <summary>
    /// Work in progress. Will allow for many points treated as one.
    /// </summary>
    public class PhysicsObject
    {
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

        /// <summary>
        /// Work in progress
        /// </summary>
        /// <param name="collidingVel">The velocity with which the colliding object impacted</param>
        /// <param name="collidingMass">The mass of the colliding object</param>
        /// <param name="bounciness">Not currently in use</param>
        public void Collision(Point collidingVel, int collidingMass, float bounciness)
        {
            Velocity += (collidingVel * collidingMass) / Mass;
        }




        public bool InteractsWithEnvironment = true;

        public PhysicsObject(bool obeysGravity, bool obeysDrag, bool interactsWithEnvironment)
        {
            
        }

        public void Update()
        {
            if (Unified)
            {

                bool hitsFloor = false;
                bool hitsLeft = false;
                bool hitsRight = false;
                bool hitsTop = false;
                Point bottomLeft = Contents.BottomLeft();
                Point topRight = Contents.TopRight();
                exteriorPoints = (List<ObjectPoint>)Contents.ExteriorPoints();


                foreach (ObjectPoint point in Contents)
                {
                    if (InteractsWithEnvironment && exteriorPoints.Contains(point) && World.Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y - 1)))
                    {
                        hitsFloor = true;
                    }
                    if (InteractsWithEnvironment && exteriorPoints.Contains(point) && World.Contents.ContainsPoint(new Point(point.Position.X, point.Position.Y + 1)))
                    {
                        hitsTop = true;
                    }
                    if (InteractsWithEnvironment && exteriorPoints.Contains(point) && World.Contents.ContainsPoint(new Point(point.Position.X - 1, point.Position.Y)))
                    {
                        hitsLeft = true;
                    }
                    if (InteractsWithEnvironment && exteriorPoints.Contains(point) && World.Contents.ContainsPoint(new Point(point.Position.X + 1, point.Position.Y)))
                    {
                        hitsRight = true;
                    }
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


    }
}
