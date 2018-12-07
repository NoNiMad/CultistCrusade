using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace MapGenerator
{
    public class GridTileMatcher
    {
        public static Optional<TileRotation> MatchPattern(MapGrid grid, int x, int y, TilePattern pattern)
        {
            if (pattern.Match(grid, x, y, Direction.NORTH))
                return new TileRotation(Direction.NORTH).Optional();
            if (pattern.Match(grid, x, y, Direction.SOUTH))
                return new TileRotation(Direction.SOUTH).Optional();
            if (pattern.Match(grid, x, y, Direction.EAST))
                return new TileRotation(Direction.EAST).Optional();
            if (pattern.Match(grid, x, y, Direction.WEST))
                return new TileRotation(Direction.WEST).Optional();

            return Optional<TileRotation>.Empty<TileRotation>();
        }
    }

    public struct TilePattern
    {
        private readonly List<TilePredicate> _predicates;

        private TilePattern(List<TilePredicate> value) : this()
        {
            _predicates = value;
        }

        public bool Match(MapGrid grid, int x, int y, Direction direction)
        {
            return _predicates.All(predicate => predicate.Invoke(grid, x, y, direction));
        }

        public static TilePattern Create(string row1, string row2, string row3)
        {
            return Create(row1 + row2 + row3);
        }

        public static TilePattern Create(string rows)
        {
            if (rows.Length != 9)
                throw new ArgumentException("Pattern must contains 9 chars!");

            var predicates = new List<TilePredicate>();

            for (var i = 0; i < rows.Length; i++)
            {
                if (rows[i] == '?')
                    continue;

                var offsetX = i % 3 - 1;
                var offsetY = i / 3 - 1;

                if (rows[i] == 'X')
                    predicates.Add((grid, x, y, direction) =>
                    {
                        switch (direction)
                        {
                            case Direction.NORTH:
                                return grid.GetTileType(x + offsetX, y + offsetY) != TileType.EMPTY;
                            case Direction.SOUTH:
                                return grid.GetTileType(x + offsetX, y - offsetY) != TileType.EMPTY;
                            case Direction.EAST:
                                return grid.GetTileType(x + offsetY, y + offsetX) != TileType.EMPTY;
                            case Direction.WEST:
                                return grid.GetTileType(x - offsetY, y + offsetX) != TileType.EMPTY;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                        }
                    });
                else if (rows[i] == ' ')
                    predicates.Add((grid, x, y, direction) =>
                    {
                        switch (direction)
                        {
                            case Direction.NORTH:
                                return grid.GetTileType(x + offsetX, y + offsetY) == TileType.EMPTY;
                            case Direction.SOUTH:
                                return grid.GetTileType(x + offsetX, y - offsetY) == TileType.EMPTY;
                            case Direction.EAST:
                                return grid.GetTileType(x + offsetY, y + offsetX) == TileType.EMPTY;
                            case Direction.WEST:
                                return grid.GetTileType(x - offsetY, y + offsetX) == TileType.EMPTY;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                        }
                    });
            }

            return new TilePattern(predicates);
        }
    }

    public delegate bool TilePredicate(MapGrid grid, int x, int y, Direction direction);
}