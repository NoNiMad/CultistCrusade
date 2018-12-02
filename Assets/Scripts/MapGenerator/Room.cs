using System;
using UnityEngine;
using Random = System.Random;

public class Room
{
    public GameObject GameObject { get; private set; }
    public Vector2 Pos;

    public Vector2 Velocity;

    private DungeonGenerator _generator;

    public Room(GameObject gameObject, DungeonGenerator generator)
    {
        GameObject = gameObject;
        _generator = generator;

        Pos.x = gameObject.transform.position.x - gameObject.transform.localScale.x / 2;
        Pos.y = gameObject.transform.position.y - gameObject.transform.localScale.y / 2;
    }

    public void Update()
    {
        Move();
        SnapToGrid();
    }

    private void Move()
    {
        Pos.x += Velocity.x;
        Pos.y += Velocity.y;

        if (GetRight() >= _generator.mapWidth / 2f)
            Pos.x = _generator.mapWidth / 2f - (GetRight() - GetLeft());
        if (GetBottom() >= _generator.mapHeight / 2f)
            Pos.y = _generator.mapHeight / 2f - (GetBottom() - GetTop());
        if (GetLeft() <= -_generator.mapWidth / 2f)
            Pos.x = -_generator.mapWidth / 2f;
        if (GetTop() <= -_generator.mapHeight / 2f)
            Pos.y = -_generator.mapHeight / 2f;
    }

    private const float speed = 1f;

    public void Repulse(Room other, Random rand)
    {
        var dx = Pos.x - other.Pos.x;
        var dy = Pos.y - other.Pos.y;

        var vector3 = other.GameObject.transform.position - GameObject.transform.position;
        vector3.Normalize();

        Velocity.x += vector3.x * rand.Next(-1, 1);
        Velocity.y += vector3.y * rand.Next(-1, 1);

        //   Velocity.x += dx * rand.Next(-1, 1);
        //  Velocity.y += dy * rand.Next(-1, 1);
    }

    public void SnapToGrid()
    {
        Pos.x = (float) Math.Round(Pos.x);
        Pos.y = (float) Math.Round(Pos.y);

        GameObject.transform.position = new Vector3(Pos.x + GameObject.transform.localScale.x / 2,
            Pos.y + GameObject.transform.localScale.y / 2, 0);
    }

    public float GetLeft()
    {
        return Pos.x;
    }

    public float GetRight()
    {
        return Pos.x + GameObject.transform.localScale.x;
    }

    public float GetTop()
    {
        return Pos.y;
    }

    public float GetBottom()
    {
        return Pos.y + GameObject.transform.localScale.y;
    }
}