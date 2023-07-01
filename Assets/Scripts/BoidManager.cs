using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField] private bool testingBoids = true;
    public int spawnLimit = 5;
    [Range(0f, 10)]
    public float cohesion = 1f;
    [Range(0f, 10)]
    public float alignment = 1f;
    [Range(0f, 20)]
    public float avoidance = 1f;
    [Range(0f, 100)]
    public float threatAvoidance = 1f;

    private List<Boid> allBoids = new List<Boid>();
    [SerializeField] private Boid prefab;
    [SerializeField] private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        if (!testingBoids)
            return;
        for (int i = 0; i < spawnLimit; i++)
        {
            var boid = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            if (i == 0)
                boid.showLog = true;
            boid.Initialize(this);
            allBoids.Add(boid);
        }
    }

    public void GenerateBoids(List<Vector2> locations)
    {
        for (int i = 0; i < locations.Count; i++)
        {
            var location = locations[i];
            var boid = Instantiate(prefab, new Vector3(location.x, location.y), Quaternion.identity, this.transform);
            boid.Initialize(this);
            allBoids.Add(boid);
        }

        UserInterface.Instance.UpdateCowCount(allBoids.Count);
    }

    public List<Boid> GetNearbyBoids(Boid b)
    {
        var nearybyBoids = new List<Boid>();
        foreach (var boid in allBoids)
        {
            if (boid == b)
                continue;

            if (Vector2.Distance(b.Position, boid.Position) < boid.viewDistance)
                nearybyBoids.Add(boid);
        }

        return nearybyBoids;
    }

    public int GetUncollectedBoidCount()
    {
        return allBoids.Where(boid => !boid.Collected).Count();
    }

    public Vector2 GetThreat()
    {
        return playerMovement.Position;
    }
}
