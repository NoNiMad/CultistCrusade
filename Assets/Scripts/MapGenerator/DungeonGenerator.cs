using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MapGenerator;
using PrimAlgorithm;
using PrimAlgorithm.GraphController;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Mesh = TriangleNet.Mesh;
using Random = System.Random;

public class DungeonGenerator : MonoBehaviour
{
    public int mapWidth = 5, mapHeight = 5;
    public IntRange roomAmount = new IntRange(5, 10);
    public IntRange roomWidth = new IntRange(10, 20);
    public IntRange roomHeight = new IntRange(10, 20);
    public int mainRooms = 3;
    public float relevantRoomFactor = 1;

    private Random rand;
    private List<Room> rooms;

    private Mesh mesh;
    private Dictionary<Graph<Vector2>.Node, Graph<Vector2>.Node> tree;
    private Graph<Vector2> finalGraph;
    private Graph<Vector2> graph;

    private MapGrid grid;

    void Start()
    {
        Stopwatch watch = new Stopwatch();

        watch.Start();

        rand = new Random();

        Color backgroundColor;
        ColorUtility.TryParseHtmlString("#2F4F4F", out backgroundColor);

        Color roomColor;
        ColorUtility.TryParseHtmlString("#DEB887", out roomColor);

        var backgroundDummyRoom = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backgroundDummyRoom.transform.position = new Vector3(0, 0, 1);
        backgroundDummyRoom.transform.localScale = new Vector3(mapWidth, mapHeight, 1);
        backgroundDummyRoom.GetComponent<MeshRenderer>().material.color = backgroundColor;

        rooms = new List<Room>();
        var relevantRooms = new List<Room>();

        for (var i = 0; i < mainRooms; i++)
            TryCreateRoom(1.25f, Color.yellow, rooms);

        relevantRooms.AddRange(rooms);
        for (var i = 0; i < roomAmount.Random - mainRooms; i++)
            TryCreateRoom(1, roomColor, rooms);

        var query = rooms.Where(room =>
                (room.Size.x > relevantRoomFactor * roomWidth.Average ||
                 room.Size.y > relevantRoomFactor * roomHeight.Average) && !relevantRooms.Contains(room))
            .ToList();
        relevantRooms.AddRange(query);

        var polygon = new Polygon();

        foreach (var room in relevantRooms)
        {
            room.IsMainRoom = true;
            var x = room.GetLeft() + (room.GetRight() - room.GetLeft()) / 2;
            var y = room.GetTop() + (room.GetBottom() - room.GetTop()) / 2;
            polygon.Add(new Vertex(x, y));
        }

        Debug.Log("Relevant rooms: " + relevantRooms.Count);
        var options =
            new ConstraintOptions {ConformingDelaunay = true, SegmentSplitting = 1};
        mesh = (Mesh) polygon.Triangulate(options);

        graph = new Graph<Vector2>(mesh.Vertices.Select(vertex => vertex.toVec2()).ToList());

        foreach (var edge in mesh.Edges)
        {
            var v0 = mesh.vertices[edge.P0].toVec2();
            var v1 = mesh.vertices[edge.P1].toVec2();
            var dist = (int) (v0 - v1).magnitude;

            graph.SetEdge(v0, v1, dist);
        }

        var prim = new Prim<Vector2>();
        tree = new Dictionary<Graph<Vector2>.Node, Graph<Vector2>.Node>();
        var mainRoomPos = new Vector2(rooms[0].GetLeft() + (rooms[0].GetRight() - rooms[0].GetLeft()) / 2,
            rooms[0].GetTop() + (rooms[0].GetBottom() - rooms[0].GetTop()) / 2);
        prim.prim(graph, graph.FindVertex(mainRoomPos), ref tree);

        finalGraph = new Graph<Vector2>(Enumerable.Empty<Vector2>());
        foreach (var pair in tree)
        {
            if (!finalGraph.Contains(pair.Key.context))
                finalGraph.AddVertex(pair.Key.context);
            if (!finalGraph.Contains(pair.Value.context))
                finalGraph.AddVertex(pair.Value.context);
            finalGraph.SetEdge(pair.Key.context, pair.Value.context, 0);
        }

        foreach (var edge in mesh.Edges)
        {
            var v0 = graph.FindVertex(mesh.vertices[edge.P0].toVec2());
            var v1 = graph.FindVertex(mesh.vertices[edge.P1].toVec2());

            if (finalGraph.Contains(v0) &&
                finalGraph.FindVertex(v0.context).edges.Any(subEdge => subEdge.to.context.Equals(v1.context)))
                continue;
            if (rand.NextDouble() > 0.25)
                continue;
            if (!finalGraph.Contains(v0.context))
                finalGraph.AddVertex(v0.context);
            if (!finalGraph.Contains(v1.context))
                finalGraph.AddVertex(v1.context);

            finalGraph.SetEdge(v0.context, v1.context, 0);
        }

        grid = new MapGrid(mapWidth, mapHeight);

        foreach (var room in relevantRooms)
        {
            GridFiller.FillRoom(grid, room);
        }

        CreateCorridors(relevantRooms);

        CreateHallwayRooms(rooms, relevantRooms);

        watch.Stop();
        Debug.Log("Generated in " + watch.ElapsedMilliseconds + " ms");
    }

    public void CreateHallwayRooms(List<Room> rooms, List<Room> relevantRooms)
    {
        var toEvict = new List<Room>();

        foreach (var room in rooms)
        {
            if (relevantRooms.Contains(room))
                continue;

            if (GridFiller.HasLinkedCorridor(grid, room))
                GridFiller.FillRoom(grid, room);
            else
                toEvict.Add(room);
        }

        rooms.RemoveAll(room => toEvict.Contains(room));
    }

    private List<Vector2> midpoints = new List<Vector2>();

    public void CreateCorridors(List<Room> relevantRooms)
    {
        var generatedEdges = new List<Tuple<Room, Room>>();

        foreach (var node in finalGraph.nodes)
        {
            var roomFrom = GetRoomByPos(node.context, relevantRooms);
            foreach (var edge in node.edges)
            {
                var roomTo = GetRoomByPos(edge.to.context, relevantRooms);

                if (generatedEdges.Any(generatedEdge =>
                    generatedEdge.Item1.Equals(roomFrom) && generatedEdge.Item2.Equals(roomTo) ||
                    generatedEdge.Item1.Equals(roomTo) && generatedEdge.Item2.Equals(roomFrom)))
                    continue;

                var midpoint = roomFrom.GetMidPointBetween(roomTo);

                midpoints.Add(midpoint);
                var isHorAligned = midpoint.x > roomFrom.GetLeft() && midpoint.x < roomFrom.GetRight() &&
                                   midpoint.x > roomTo.GetLeft() && midpoint.x < roomTo.GetRight();
                var isVerAligned = midpoint.y > roomFrom.GetTop() && midpoint.y < roomFrom.GetBottom() &&
                                   midpoint.y > roomTo.GetTop() && midpoint.y < roomTo.GetBottom();

                generatedEdges.Add(new Tuple<Room, Room>(roomFrom, roomTo));
                if (isHorAligned)
                {
                    Debug.Log("Corridor VER");
                    if (roomTo.GetTop() < roomFrom.GetTop())
                        GridFiller.FillCorridor(grid, new Vector2(midpoint.x, roomTo.GetBottom()),
                            new Vector2(midpoint.x, roomFrom.GetTop()), 3, Orientation.VERTICAL);
                    else
                        GridFiller.FillCorridor(grid, new Vector2(midpoint.x, roomFrom.GetBottom()),
                            new Vector2(midpoint.x, roomTo.GetTop()), 3, Orientation.VERTICAL);
                    continue;
                }

                if (isVerAligned)
                {
                    Debug.Log("Corridor HOR");

                    if (roomTo.GetLeft() < roomFrom.GetLeft())
                        GridFiller.FillCorridor(grid, new Vector2(roomTo.GetRight(), midpoint.y),
                            new Vector2(roomFrom.GetLeft(), midpoint.y), 3, Orientation.HORIZONTAL);
                    else
                        GridFiller.FillCorridor(grid, new Vector2(roomFrom.GetRight(), midpoint.y),
                            new Vector2(roomTo.GetLeft(), midpoint.y), 3, Orientation.HORIZONTAL);
                    continue;
                }


                // RoomTo is BOTTOM-RIGHT
                if (roomTo.GetTop() > roomFrom.GetTop() && roomTo.GetLeft() > roomFrom.GetLeft())
                {
                    GridFiller.FillCorridor(grid, new Vector2(roomFrom.GetRight(), roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y), 3, Orientation.HORIZONTAL);
                    GridFiller.FillCorridor(grid, new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomTo.GetTop()), 3, Orientation.VERTICAL);
                }

                // RoomTo is TOP-RIGHT
                if (roomTo.GetTop() < roomFrom.GetTop() && roomTo.GetLeft() > roomFrom.GetLeft())
                {
                    GridFiller.FillCorridor(grid, new Vector2(roomFrom.GetRight(), roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y), 3, Orientation.HORIZONTAL);
                    GridFiller.FillCorridor(grid, new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomTo.GetBottom()), 3, Orientation.VERTICAL);
                }

                // RoomTo is BOTTOM-LEFT
                if (roomTo.GetTop() > roomFrom.GetTop() && roomTo.GetLeft() < roomFrom.GetLeft())
                {
                    GridFiller.FillCorridor(grid, new Vector2(roomFrom.GetLeft(), roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y), 3, Orientation.HORIZONTAL);
                    GridFiller.FillCorridor(grid, new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomTo.GetTop()), 3, Orientation.VERTICAL);
                }

                // RoomTo is TOP-LEFT
                if (roomTo.GetTop() < roomFrom.GetTop() && roomTo.GetLeft() < roomFrom.GetLeft())
                {
                    GridFiller.FillCorridor(grid, new Vector2(roomFrom.GetLeft(), roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y), 3, Orientation.HORIZONTAL);
                    GridFiller.FillCorridor(grid, new Vector2(roomTo.GetMidPoint().x, roomFrom.GetMidPoint().y),
                        new Vector2(roomTo.GetMidPoint().x, roomTo.GetBottom()), 3, Orientation.VERTICAL);
                }

                Debug.Log("Corridor L");
            }
        }
    }

    private Room GetRoomByPos(Vector2 pos, List<Room> rooms)
    {
        return rooms.FindLast(room =>
            room.GetLeft() <= pos.x && room.GetRight() >= pos.x && room.GetTop() <= pos.y && room.GetBottom() >= pos.y);
    }

    public void OnDrawGizmos()
    {
        if (mesh == null)
        {
            // We're probably in the editor
            return;
        }

        for (var x = 0; x < grid.GetWidth(); x++)
        {
            for (var y = 0; y < grid.GetHeight(); y++)
            {
                switch (grid.GetTileType(x, y))
                {
                    case TileType.ROOM:
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(new Vector3(x - mapWidth / 2 + 0.5f, y - mapHeight / 2 + 0.5f, -1),
                            new Vector3(1, 1, 0));
                        break;
                    case TileType.HALLWAY:
                        Gizmos.color = Color.white;
                        Gizmos.DrawCube(new Vector3(x - mapWidth / 2 + 0.5f, y - mapHeight / 2 + 0.5f, -1),
                            new Vector3(1, 1, 0));
                        break;
                    case TileType.HALLWAY_ROOM:
                        Gizmos.color = Color.grey;
                        Gizmos.DrawCube(new Vector3(x - mapWidth / 2 + 0.5f, y - mapHeight / 2 + 0.5f, -1),
                            new Vector3(1, 1, 0));
                        break;
                    case TileType.EMPTY:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        Gizmos.color = Color.blue;

        foreach (var node in finalGraph.nodes)
        {
            foreach (var edge in node.edges)
            {
                var p0 = new Vector3(node.context.x, node.context.y, 0);
                var p1 = new Vector3(edge.to.context.x, edge.to.context.y, 0);
                Gizmos.DrawLine(p0, p1);
            }
        }

        Gizmos.color = Color.red;

        foreach (var pair in tree)
        {
            var p0 = new Vector3(pair.Key.context.x, pair.Key.context.y, 0);
            var p1 = new Vector3(pair.Value.context.x, pair.Value.context.y, 0);
            Gizmos.DrawLine(p0, p1);
        }

        foreach (var midpoint in midpoints)
        {
            Gizmos.DrawCube(new Vector3(midpoint.x, midpoint.y, -1), new Vector3(1, 1, 1));
        }
    }

    private void TryCreateRoom(float randomModifier, Color roomColor, ICollection<Room> rooms)
    {
        var tryLeft = 100;

        Room room = null;
        while (room == null || GetCollidings(room, rooms).Count != 0 ||
               room.GetLeft() < -mapWidth / 2f || room.GetRight() > mapWidth / 2f ||
               room.GetTop() < -mapHeight / 2f || room.GetBottom() > mapHeight / 2f)
        {
            if (tryLeft <= 0)
                return;

            var pos = GetRandomPointInCircle((float) mapWidth / 2);
            room = new Room((int) pos.x, (int) pos.y,
                (int) (roomWidth.Random * randomModifier), (int) (roomHeight.Random * randomModifier));

            tryLeft--;
        }

        rooms.Add(room);
    }

    void Update()
    {
    }

    List<Room> GetCollidings(Room room, IEnumerable<Room> colliders)
    {
        var collidings = new List<Room>();

        foreach (var otherRoom in colliders)
        {
            if (room == otherRoom)
                continue;

            if (!(room.GetLeft() <= otherRoom.GetRight()) || !(room.GetRight() >= otherRoom.GetLeft()) ||
                !(room.GetTop() <= otherRoom.GetBottom()) || !(room.GetBottom() >= otherRoom.GetTop())) continue;
            if (!collidings.Contains(room))
                collidings.Add(room);
            break;
        }

        return collidings;
    }

    Vector2 GetRandomPointInCircle(float radius)
    {
        var t = 2 * Math.PI * rand.NextDouble();
        var u = rand.NextDouble() + rand.NextDouble();
        double r;

        if (u > 1)
            r = 2 - u;
        else
            r = u;
        return new Vector2(Roundm((float) (radius * r * Math.Cos(t)), 1),
            Roundm((float) (radius * r * Math.Sin(t)), 1));
    }

    float Roundm(float n, float m)
    {
        return (float) (Math.Floor((n + m - 1) / m) * m);
    }
}