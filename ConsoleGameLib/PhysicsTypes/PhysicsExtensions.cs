using ConsoleGameLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleGameLib.PhysicsTypes
{
    public static class PhysicsExtensions
    {
        public static bool ContainsPoint(this IEnumerable<PhysicsPoint> points, Point point, bool mustInteractWithEnvironment = true)
        {
            if (points != null)
            {
                foreach (PhysicsPoint entry in points)
                {
                    if (entry.Position == point && (mustInteractWithEnvironment ? entry.InteractsWithEnvironment : true))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ContainsPoint(this IEnumerable<ObjectPoint> points, Point point)
        {
            foreach (ObjectPoint entry in points)
            {
                if (entry.Position == point)
                {
                    return true;
                }
            }
            return false;
        }

        public static Point BottomLeft(this IEnumerable<ObjectPoint> points)
        {
            Point lowest = new Point(int.MaxValue,int.MaxValue);

            foreach(ObjectPoint point in points)
            {
                if(point.Position.X < lowest.X)
                {
                    lowest.X = point.Position.X;
                }
                if(point.Position.Y < lowest.Y)
                {
                    lowest.Y = point.Position.Y;
                }
            }
            return lowest;
        }

        public static Point TopRight(this IEnumerable<ObjectPoint> points)
        {
            Point highest = new Point(int.MinValue,int.MinValue);

            foreach (ObjectPoint point in points)
            {
                if (point.Position.X > highest.X)
                {
                    highest.X = point.Position.X;
                }
                if (point.Position.Y > highest.Y)
                {
                    highest.Y = point.Position.Y;
                }
            }
            return highest;
        }

        public static IEnumerable<ObjectPoint> InteriorPoints(this IEnumerable<ObjectPoint> points)
        {
            List<ObjectPoint> interiorPts = new List<ObjectPoint>();
            foreach (ObjectPoint point in points)
            {
                if (point.IsInterior(points))
                {
                    interiorPts.Add(point);
                }
            }
            return interiorPts;

        }
        public static IEnumerable<ObjectPoint> ExteriorPoints(this IEnumerable<ObjectPoint> points)
        {
            List<ObjectPoint> exteriorPts = new List<ObjectPoint>();
            foreach(ObjectPoint point in points)
            {
                if(!point.IsInterior(points))
                {
                    exteriorPts.Add(point);
                }
            }
            return exteriorPts;

        }

        public static bool IsInterior(this ObjectPoint point, IEnumerable<ObjectPoint> group)
        {
            return group.ContainsPoint(new Point(point.Position.X + 1, point.Position.Y)) && group.ContainsPoint(new Point(point.Position.X - 1, point.Position.Y)) && group.ContainsPoint(new Point(point.Position.X, point.Position.Y + 1)) && group.ContainsPoint(new Point(point.Position.X, point.Position.Y - 1));
        }
    }
}
