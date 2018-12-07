using System;
using UnityEngine;

namespace MapGenerator
{
    public class TileRotation
    {
        private readonly Direction _direction;

        public TileRotation(Direction direction)
        {
            _direction = direction;
        }

        public Quaternion ToQuat()
        {
            switch (_direction)
            {
                case Direction.NORTH:
                    return Quaternion.Euler(new Vector3(0, 0, 0));
                case Direction.EAST:
                    return Quaternion.Euler(new Vector3(0, 90, 0));
                case Direction.SOUTH:
                    return Quaternion.Euler(new Vector3(0, 180, 0));
                case Direction.WEST:
                    return Quaternion.Euler(new Vector3(0, -90, 0));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}