using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils
{
    public static Vector2Int ToGrid(Vector2 position)
    {
        return new Vector2Int((int)position.x / 100 - 1, (int)position.y / 100 - 3);
    }
    public static Vector2 FromGrid(Vector2Int grid)
    {
        return new Vector2Int((int)grid.x * 100, (int)grid.y * 100);
    }
}