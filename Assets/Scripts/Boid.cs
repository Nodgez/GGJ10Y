using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public bool showLog = false;
    public float wallDistanceCheck = 3;
    public float viewDistance = 10f;
    public float avoidanceDistance = 1f;
    public float threatAvoidanceDistance = 1f;
    private Vector2 position;
    public Vector2 velocity;
    private BoidManager boidManager;
    public LayerMask avoidanceLayer;

    public float threatMeter = 0;

    private Ray2D[] rays = new Ray2D[6];


    public Vector2 Position
    {
        get { return position; }
    }

    public Vector3 Direction
    {
        get { return velocity.normalized; }
    }

    public bool Collected
    {
        get;
        private set;
    }

    public void Initialize(BoidManager boidManager)
    {
        this.boidManager = boidManager;
        velocity = new Vector2(Random.value, Random.value).normalized;
    }

    private void Update()
    {
        if (Collected)
            return;
        velocity += Flock().normalized;
        //velocity += Align().normalized;
        velocity += Avoid().normalized;
        velocity += Predator().normalized;
        velocity += Wall().normalized;
        transform.position += new Vector3(velocity.x, velocity.y, 0).normalized * Time.deltaTime * Mathf.Lerp(2, 5f, threatMeter);

        position = new Vector2(transform.position.x, transform.position.y);
        velocity = Vector2.zero;
    }

    private Vector2 Avoid()
    {
        var nearbyBoids = boidManager.GetNearbyBoids(this);
        var delta = Vector2.zero;

        foreach (var neighbor in nearbyBoids)
        {
            float closeness = Mathf.Max(0, avoidanceDistance - Vector2.Distance(this.position, neighbor.position));
            delta += (this.Position - neighbor.Position) * closeness;
        }
        return delta * boidManager.avoidance;
    }

    private Vector2 Flock()
    {
        var nearbyBoids = boidManager.GetNearbyBoids(this);
        if (nearbyBoids.Count == 0)
            return Vector2.zero;

        var mean = Vector2.zero;
        var delta = Vector2.zero;

        foreach (var neighbor in nearbyBoids)
            mean += neighbor.position;

        mean /= nearbyBoids.Count;
        delta = mean - position;

        return delta.magnitude > 0.2f ? delta * boidManager.cohesion : Vector2.zero;//(Mathf.Lerp(boidManager.cohesion, boidManager.cohesion * 0.1f, threatMeter));
    }

    private Vector2 Align()
    {
        var nearbyBoids = boidManager.GetNearbyBoids(this);
        var mean = Vector2.zero;
        var delta = Vector2.zero;

        foreach (var neighbor in nearbyBoids)
            mean += neighbor.velocity;

        if (nearbyBoids.Count < 1)
            return Vector2.zero;

        mean /= nearbyBoids.Count;
        delta = mean - velocity;
        return delta * boidManager.alignment;//(Mathf.Lerp(boidManager.alignment, boidManager.alignment * 0.1f, threatMeter));
    }

    private Vector2 Predator()
    {
        var threat = boidManager.GetThreat();
        var delta = Vector2.zero;
        float closeness = Mathf.Max(0, threatAvoidanceDistance - Vector2.Distance(this.position, threat));
        threatMeter = Mathf.Lerp(1f, 0f, 1f - (closeness / (avoidanceDistance)));
        delta = (this.Position - threat) * (closeness / threatAvoidanceDistance);
        return delta * boidManager.threatAvoidance;
    }

    private Vector2 Wall()
    {
        CalculateWallDetectionRays();

        var delta = Vector2.zero;
        foreach (var r in rays)
        {
            var raycastHit = Physics2D.Raycast(r.origin, r.direction, wallDistanceCheck, avoidanceLayer);
            if (raycastHit.collider == null)
            {
                Log("no walls detected");
                continue;
            }

            var rCollider = raycastHit.collider.transform;
            var asV2 = new Vector2(rCollider.position.x, rCollider.position.y);
            float closeness = Mathf.Max(0, wallDistanceCheck - Vector2.Distance(this.position, asV2));
            delta += (this.Position - asV2) * closeness;
        }
        Log(delta.ToString());
        return delta * 100;
    }

    private void CalculateWallDetectionRays()
    {
        var increment = 360f / 6f;//rays.Length;
        for (int i = 0; i < rays.Length; i++)
        {
            var angle = i * increment;
            var x = Mathf.Cos(angle * Mathf.Deg2Rad);
            var y = Mathf.Sin(angle * Mathf.Deg2Rad);
            var heading = new Vector2(x, y) * wallDistanceCheck;
            var location = new Vector2(transform.position.x, transform.position.y);
            //Debug.DrawLine(new Vector3(position.x, position.y), new Vector3(position.x + heading.x, position.y + heading.y), Color.yellow);
            rays[i] = new Ray2D(location, heading);
        }
    }

    public void Collect()
    {
        Collected = true;
        UserInterface.Instance.UpdateCowCount(boidManager.GetUncollectedBoidCount());
    }

    private void Log(string text)
    {
        if (showLog)
            print(text);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!showLog)
            return;
        var threat = boidManager.GetThreat();
        var threat3D = new Vector3(threat.x, threat.y);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, threat3D);

        var p = Predator();
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, new Vector3(p.x, p.y));
        Handles.Label(Vector3.Lerp(this.transform.position, threat3D, 0.5f), Vector2.Distance(position, threat).ToString());

        var nearbyBoids = boidManager.GetNearbyBoids(this);
        foreach (var b in nearbyBoids)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, b.transform.position);
        }

        foreach (var r in rays)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(r.origin, r.direction);
        }
    }
#endif
}
