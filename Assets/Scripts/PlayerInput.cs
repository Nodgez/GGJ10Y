using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerActions actions;
    private Vector2 directon;
    public Vector3 Directon
    {
        get { return directon.normalized; }
    }

    public Vector3 Directon_x
    {
        get { return new Vector3(directon.x, 0, 0).normalized; }
    }

    public Vector3 Directon_y
    {
        get { return new Vector3(0, directon.y, 0).normalized; }
    }

    private void Awake()
    {
        actions = new PlayerActions();
    }

    // Update is called once per frame
    void Update()
    {
        var input = actions.Player_Map.Movement.ReadValue<Vector2>();
        directon = new Vector3(input.x, input.y, 0);
    }

    private void OnEnable()
    {
        actions.Enable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}
