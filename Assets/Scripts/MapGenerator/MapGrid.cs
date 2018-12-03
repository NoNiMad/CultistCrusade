using System;

namespace MapGenerator
{
    public class MapGrid
    {
        private readonly int _width, _height;
        private int[,] Grid { get; }

        public MapGrid(int width, int height)
        {
            Grid = new int[width, height];

            _width = width;
            _height = height;
        }

        public int GetWidth()
        {
            return Grid.GetLength(0);
        }

        public int GetHeight()
        {
            return Grid.GetLength(1);
        }

        public void SetTileType(int x, int y, TileType type)
        {
            if (x >= _width || y >= _height || x < 0 || y < 0)
                return;

            switch (type)
            {
                case TileType.ROOM:
                    Grid[x, y] = 1;
                    break;
                case TileType.HALLWAY:
                    Grid[x, y] = 2;
                    break;
                case TileType.HALLWAY_ROOM:
                    Grid[x, y] = 3;
                    break;
                case TileType.EMPTY:
                    Grid[x, y] = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public TileType GetTileType(int x, int y)
        {
            if (x >= _width || y >= _height || x < 0 || y < 0)
                return TileType.EMPTY;

            switch (Grid[x, y])
            {
                case 0:
                    return TileType.EMPTY;
                case 1:
                    return TileType.ROOM;
                case 2:
                    return TileType.HALLWAY;
                case 3:
                    return TileType.HALLWAY_ROOM;
                default:
                    return TileType.EMPTY;
            }
        }
    }

    public enum TileType
    {
        ROOM,
        HALLWAY,
        HALLWAY_ROOM,
        EMPTY
    }
}