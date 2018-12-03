using UnityEngine;

public class Room
{
    public Vector2 Pos;
    public Vector2 Size;
    public bool IsMainRoom { get; set; }
    public bool IsSpawn;
    public bool IsBoss;

    public Room(int x, int y, int width, int height)
    {
        Pos = new Vector2(x, y);
        Size = new Vector2(width, height);
    }

    public float GetLeft()
    {
        return Pos.x;
    }

    public float GetRight()
    {
        return Pos.x + Size.x;
    }

    public float GetTop()
    {
        return Pos.y;
    }

    public float GetBottom()
    {
        return Pos.y + Size.y;
    }

    public float GetWidth()
    {
        return Size.x;
    }

    public float GetHeight()
    {
        return Size.y;
    }

    public Vector2 GetMidPoint()
    {
        return Pos + Size / 2;
    }

    public Vector2 GetMidPointBetween(Room other)
    {
        var midpoint = new Vector2();

        if (other.GetLeft() + other.GetWidth() / 2 > GetLeft() + GetWidth() / 2)
            midpoint.x = GetLeft() + GetWidth() / 2 +
                         (other.GetLeft() + other.GetWidth() / 2 - (GetLeft() + GetWidth() / 2)) / 2;
        else
            midpoint.x = other.GetLeft() + other.GetWidth() / 2 +
                         (GetLeft() + GetWidth() / 2 - (other.GetLeft() + other.GetWidth() / 2)) / 2;

        if (other.GetTop() + other.GetHeight() / 2 > GetTop() + GetHeight() / 2)
            midpoint.y = GetTop() + GetHeight() / 2 +
                         (other.GetTop() + other.GetHeight() / 2 - (GetTop() + GetHeight() / 2)) / 2;
        else
            midpoint.y = other.GetTop() + other.GetHeight() / 2 +
                         (GetTop() + GetHeight() / 2 - (other.GetTop() + other.GetHeight() / 2)) / 2;

        return midpoint;
    }
}