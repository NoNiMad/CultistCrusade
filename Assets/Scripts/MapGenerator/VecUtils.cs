using TriangleNet.Geometry;
using UnityEngine;

namespace MapGenerator
{
    public static class VecUtils
    {
        public static Vector2 toVec2(this Vertex self)
        {
            return new Vector2((float) self.x, (float) self.y);
        }
    }
}