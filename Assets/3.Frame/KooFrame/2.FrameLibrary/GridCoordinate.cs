using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct GridCoordinate
{
    public int x, y;

    public GridCoordinate (int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public GridCoordinate(Vector2 pos)
    {
        this.x = Mathf.FloorToInt(pos.x);
        this.y = Mathf.FloorToInt(pos.y);
    }
    public static implicit operator Vector2(GridCoordinate c)
    {
        return new Vector2(c.x, c.y);
    }
    public static implicit operator Vector2Int(GridCoordinate c)
    {
        return new Vector2Int(c.x, c.y);
    }   
    public static implicit operator Vector3(GridCoordinate c)
    {
        return new Vector3(c.x, c.y, 0f);
    }
    public static implicit operator Vector3Int(GridCoordinate c)
    {
        return new Vector3Int(c.x, c.y, 0);
    }
    public static explicit operator GridCoordinate(Vector3 pos)
    {
        return new GridCoordinate(pos);
    }
    public static explicit operator GridCoordinate(Vector2 pos)
    {
        return new GridCoordinate(pos);
    }
    public static explicit operator GridCoordinate(Vector2Int pos)
    {
        return new GridCoordinate(pos);
    }
    public static GridCoordinate operator +(GridCoordinate c1, GridCoordinate c2)
    {
        return new GridCoordinate(c1.x+c2.x,c1.y+c2.y);
    }
    public static GridCoordinate operator -(GridCoordinate c1, GridCoordinate c2)
    {
        return new GridCoordinate(c1.x - c2.x, c1.y - c2.y);
    }
    public static bool operator ==(GridCoordinate c1, GridCoordinate c2)
    {
        return c1.x==c2.x&& c1.y==c2.y;
    }
    public static bool operator !=(GridCoordinate c1, GridCoordinate c2)
    {
        return c1.x != c2.x || c1.y != c2.y;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "("+x+","+y+")";
    }
}
