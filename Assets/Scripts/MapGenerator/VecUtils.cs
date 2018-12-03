using System;
using TriangleNet.Geometry;
using UnityEngine;
using Random = System.Random;

namespace MapGenerator
{
    public static class VecUtils
    {
        public static Vector2 toVec2(this Vertex self)
        {
            return new Vector2((float) self.x, (float) self.y);
        }

        public static Vector2 GetRandomPointInCircle(Random rand, float radius)
        {
            var t = 2 * Math.PI * rand.NextDouble();
            var u = rand.NextDouble() + rand.NextDouble();
            double r;

            if (u > 1)
                r = 2 - u;
            else
                r = u;
            return new Vector2(Roundm((float) (radius * r * Math.Cos(t)), 1), Roundm((float) (radius * r * Math.Sin(t)), 1));
        }

        private static float Roundm(float n, float m)
        {
            return (float) (Math.Floor((n + m - 1) / m) * m);
        }
    }
}