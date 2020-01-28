using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    // properties
    public int x { get; set; } 
    public int y { get; set; }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // C# operators
    public static bool operator ==(Point first, Point second)
    {
        return first.x == second.x && first.y == second.y;
    }

    public static bool operator !=(Point first, Point second)
    {
        return first.x != second.x || first.y != second.y;
    }

    public static Point operator -(Point x, Point y)
    {
        return new Point(x.x - y.x, x.y - y.y);
    }
}