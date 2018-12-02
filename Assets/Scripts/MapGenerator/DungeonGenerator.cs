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

    private Random rand;
    private GameObject dungeonHolder;
    private List<Room> rooms;

    private Mesh mesh;
    private Dictionary<Graph<Vector2>.Node, Graph<Vector2>.Node> tree;
    private Graph<Vector2> finalGraph;
    private Graph<Vector2> graph;

    void Start()
    {
        Stopwatch watch = new Stopwatch();

        watch.Start();

        dungeonHolder = new GameObject("DungeonHolder");

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
                (room.GameObject.transform.localScale.x > roomWidth.Average ||
                 room.GameObject.transform.localScale.y > roomHeight.Average) && !relevantRooms.Contains(room))
            .ToList();
        relevantRooms.AddRange(query);

        var polygon = new Polygon();

        foreach (var room in relevantRooms)
        {
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
            Debug.Log("Readded");
        }

        watch.Stop();
        Debug.Log("Generated in " + watch.ElapsedMilliseconds + " ms");
    }

    public void OnDrawGizmos()
    {
        if (mesh == null)
        {
            // We're probably in the editor
            return;
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
    }

    private void TryCreateRoom(float randomModifier, Color roomColor, ICollection<Room> rooms)
    {
        int tryLeft = 100;

        Room room = null;
        while (room == null || GetCollidings(room, rooms).Count != 0 ||
               room.GetLeft() < -mapWidth / 2f || room.GetRight() > mapWidth / 2f ||
               room.GetTop() < -mapHeight / 2f || room.GetBottom() > mapHeight / 2f)
        {
            if (room != null)
                Destroy(room.GameObject);
            if (tryLeft <= 0)
                return;

            var pos = GetRandomPointInCircle((float) mapWidth / 2);
            room = new Room(CreateRoom((int) pos.x, (int) pos.y, 0, (int) (roomWidth.Random * randomModifier),
                (int) (roomHeight.Random * randomModifier), roomColor), this);

            tryLeft--;
        }

        rooms.Add(room);
        room.GameObject.transform.parent = dungeonHolder.transform;
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

            if (room.GetLeft() <= otherRoom.GetRight()
                && room.GetRight() >= otherRoom.GetLeft()
                && room.GetTop() <= otherRoom.GetBottom()
                && room.GetBottom() >= otherRoom.GetTop())
            {
                if (!collidings.Contains(room))
                    collidings.Add(room);

                otherRoom.Repulse(room, rand);
                break;
            }

            otherRoom.Velocity *= 0;
        }

        return collidings;
    }

    GameObject CreateRoom(int x, int y, int z, int width, int height, Color color)
    {
        var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        quad.transform.position = new Vector3(x, y, z);
        quad.transform.localScale = new Vector3(width, height, 1);

        quad.GetComponent<MeshRenderer>().material.color = color;

        return quad;
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