using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCell
{
    public Vector2Int location;
    public Color value;

    public GameObject gameObject;

    private List<BaseCell> neighbors = new List<BaseCell>();
    public bool IsFrontier
    {
        set;
        get;
    }

    public bool IsZoned
    {
        set;
        get;
    }

    public BaseCell(Vector2Int location, Color value)
    {
        this.location = location;
        this.value = value;
    }

    public Vector2Int GetCellLocation()
    {
        return location;
    }

    public void AddNeighbor(BaseCell cell)
    {
        if (neighbors.Contains(cell))
            return;
        neighbors.Add(cell);
    }
}