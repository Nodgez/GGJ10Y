using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed = 3f;

    [SerializeField] private float collisionRecoil = 5f;

    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    void Update()
    {
        var x = this.transform.position.x;
        var y = this.transform.position.y;

        var newPosition = transform.position + playerInput.Directon * speed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, 0, 100), Mathf.Clamp(newPosition.y, 0, 100));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var distance = Vector3.Distance(other.transform.position, transform.position);
        var collisionOffset = playerInput.Directon * -speed * collisionRecoil * Time.deltaTime;
        var newPosition = transform.position + collisionOffset;
        transform.position = new Vector3(Mathf.Clamp(newPosition.x, 0, 100), Mathf.Clamp(newPosition.y, 0, 100));
    }
}
