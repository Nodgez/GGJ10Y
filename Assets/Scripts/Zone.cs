using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zone
{
    private List<BaseCell> cells;
    public int Size
    {
        get { return cells.Count; }
    }
    public Zone(List<BaseCell> cells)
    {
        this.cells = cells;
    }

    public void Remove()
    {
        foreach (var c in cells)
        {
            c.IsFrontier = false;
            c.IsZoned = false;
            if (c.gameObject != null)
                UnityEngine.GameObject.Destroy(c.gameObject);
        }
    }

    public void GenerateCattle(BoidManager boidManager)
    {
        var cowCount = Size * 0.02f;
        var copyOfCellArray = cells.Where(cell => !cell.IsFrontier).ToList();
        var locations = new List<Vector2>();
        while (cowCount > 0)
        {
            var r = Random.Range(0, copyOfCellArray.Count);
            var cellLocation = copyOfCellArray[r].location;
            Debug.Log("Creating cow at: " + cellLocation);
            locations.Add(cellLocation);
            copyOfCellArray.RemoveAt(r);
            cowCount--;
        }

        boidManager.GenerateBoids(locations);
    }
}