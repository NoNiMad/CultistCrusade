using System;
using UnityEngine;

namespace MapGenerator
{
    public static class GridFiller
    {
        public static void FillRoom(MapGrid grid, Room room)
        {
            var left = (int) room.GetLeft() + grid.GetWidth() / 2;
            var right = (int) room.GetRight() + grid.GetWidth() / 2;
            var top = (int) room.GetTop() + grid.GetHeight() / 2;
            var bottom = (int) room.GetBottom() + grid.GetHeight() / 2;

            for (var x = left; x < right; x++)
            {
                for (var y = top; y < bottom; y++)
                {
                    if (room.IsMainRoom)
                        grid.SetTileType(x, y, TileType.ROOM);
                    else if (grid.GetTileType(x, y) == TileType.EMPTY)
                        grid.SetTileType(x, y, TileType.HALLWAY_ROOM);
                }
            }
        }

        public static void FillCorridor(MapGrid grid, Vector2 @from, Vector2 to, int width, Orientation ori)
        {
            var left = (int) Math.Min(from.x, to.x) + grid.GetWidth() / 2;
            var right = (int) Math.Max(from.x, to.x) + grid.GetWidth() / 2;
            var top = (int) Math.Min(from.y, to.y) + grid.GetHeight() / 2;
            var bottom = (int) Math.Max(from.y, to.y) + grid.GetHeight() / 2;

            left -= width / 2;
            right += width / 2;
            top -= width / 2;
            bottom += width / 2;

            for (var x = left; x < right; x++)
            {
                for (var y = top; y < bottom; y++)
                {
                    if (grid.GetTileType(x, y) == TileType.EMPTY || grid.GetTileType(x, y) == TileType.HALLWAY_ROOM)
                        grid.SetTileType(x, y, TileType.HALLWAY);
                }
            }
        }

        public static bool HasLinkedCorridor(MapGrid grid, Room room)
        {
            var left = (int) room.GetLeft() + grid.GetWidth() / 2 - 1;
            var right = (int) room.GetRight() + grid.GetWidth() / 2 + 1;
            var top = (int) room.GetTop() + grid.GetHeight() / 2 - 1;
            var bottom = (int) room.GetBottom() + grid.GetHeight() / 2 + 1;

            for (var x = left; x < right; x++)
            {
                for (var y = top; y < bottom; y++)
                {
                    var tile = grid.GetTileType(x, y);
                    if (tile == TileType.ROOM || tile == TileType.HALLWAY)
                        return true;
                }
            }
            return false;
        }
    }
}