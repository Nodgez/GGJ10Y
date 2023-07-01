using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pale, irishZone, nonIrishZone;
    [SerializeField] private Boid cowPrefab;

    private Queue<Vector2Int> frontier = new Queue<Vector2Int>();
    private List<Vector2Int> searched = new List<Vector2Int>();

    private Texture2D mapImage;

    private BaseCell[] cellArray;
    private List<Zone> cowZones = new List<Zone>();

    private int width, height, zoneCount;

    public void Initialize(Texture2D mapImage, BoidManager boidManager)
    {
        this.width = mapImage.width;
        this.height = mapImage.height;
        this.mapImage = mapImage;
        cellArray = new BaseCell[width * height];

        for (int p_i = 0; p_i < cellArray.Length; p_i++)
        {
            var colorIndex = ConvertCellIndex(p_i);
            var colorValue = mapImage.GetPixel(colorIndex.x, colorIndex.y);
            cellArray[p_i] = new BaseCell(colorIndex, colorValue);
        }

        //find all of the black pixel zones and do a flood fill to get the frontier
        foreach (var c in cellArray)
        {
            //create an outer wall
            if (c.location.x == 0 || c.location.x == width - 1 || c.location.y == 0 || c.location.y == height - 1)
                c.gameObject = Instantiate(pale, new Vector3(c.location.x, c.location.y), Quaternion.identity);
            if (c.value != Color.black || c.IsZoned)
            {
                continue;
            }
            var zone = FloodSearch(c);
            if (zone.Size > 10)
            {
                cowZones.Add(zone);
                zone.GenerateCattle(boidManager);
            }
            else
                zone.Remove();
        }
    }

    public void Refresh()
    {

    }

    private List<int> GetNeighborIndices(Vector2Int cellLocation)
    {
        var neighborIndices = new List<int>();
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.left));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.right));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.up));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.down));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.left + Vector2Int.up));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.right + Vector2Int.up));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.left + Vector2Int.down));
        neighborIndices.Add(ConvertCellIndex(cellLocation + Vector2Int.right + Vector2Int.down));
        return neighborIndices;
    }

    private Vector2Int ConvertCellIndex(int index)
    {
        //x = index / width
        //y = index % height

        //x = 10 % 4 = 2
        //y = 10 / 5 = 2    

        return new Vector2Int(index % width, Mathf.FloorToInt(index / height));
    }

    private int ConvertCellIndex(Vector2Int index)
    {
        //x + (width * y) = index
        //0 + (4 * 0) == 0
        //2 + (4 * 2) == 10

        if (index.x < 0 || index.y < 0 || index.x >= width || index.y >= height)
            return -1;

        return index.x + (width * index.y);
    }

    public Zone FloodSearch(BaseCell startCell)
    {
        var frontier = new Queue<BaseCell>();
        var searchedList = new List<BaseCell>();

        frontier.Enqueue(startCell);
        searchedList.Add(startCell);
        var zoneTransform = new GameObject("Zone").transform;
        while (frontier.Count > 0)
        {
            var currentCell = frontier.Dequeue();
            var neightborIndices = GetNeighborIndices(currentCell.GetCellLocation());
            currentCell.IsZoned = true;
            foreach (var index in neightborIndices)
            {
                if (index == -1)
                    continue;

                var neighbor = cellArray[index];
                if (neighbor.value != currentCell.value && !neighbor.IsFrontier)
                {
                    searchedList.Add(cellArray[index]);
                    neighbor.IsFrontier = true;

                    var frontierLocation = ConvertCellIndex(index);
                    if (neighbor.gameObject == null)
                        neighbor.gameObject = Instantiate(pale, new Vector3(frontierLocation.x, frontierLocation.y, 0), Quaternion.identity, zoneTransform);
                    continue;
                }
                else
                {

                    //add each other as neighbors
                    currentCell.AddNeighbor(cellArray[index]);
                    cellArray[index].AddNeighbor(currentCell);

                    if (searchedList.Contains(cellArray[index]))
                        continue;

                    frontier.Enqueue(cellArray[index]);
                    searchedList.Add(cellArray[index]);
                }
            }
        }

        return new Zone(searchedList);
    }

    private HashSet<int> GenerateDangerLocations(int limit, int dangerCount)
    {
        //if we have bad data return an empty set
        if (dangerCount > limit)
            return new HashSet<int>();

        var randomLocations = new HashSet<int>(dangerCount);
        while (randomLocations.Count < dangerCount)
            randomLocations.Add(Random.Range(0, limit));

        return randomLocations;
    }
}
