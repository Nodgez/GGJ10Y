using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScreenBound : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed = 3f;

    private Vector3 screenMin, screenMax;

    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    private void Start()
    {
        screenMin = Camera.main.ViewportToWorldPoint(Vector3.zero);
        screenMax = Camera.main.ViewportToWorldPoint(Vector3.one);
    }

    void Update()
    {
        var x = this.transform.position.x;
        var y = this.transform.position.y;

        var newPosition = transform.position + playerInput.Directon * speed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, screenMin.x, screenMax.x), Mathf.Clamp(newPosition.y, screenMin.y, screenMax.y));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Player Triggered");
        var newPosition = transform.position + playerInput.Directon * (-speed) * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, screenMin.x, screenMax.x), Mathf.Clamp(newPosition.y, screenMin.y, screenMax.y));
    }
}
