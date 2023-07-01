using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pale : MonoBehaviour
{
    public int HP;

    void Start()
    {
        HP = Random.Range(6, 12);
    }
    [SerializeField] private FlashIndicator flashIndicator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        flashIndicator.Indicate();
        HP--;

        if (HP <= 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.IsTouchingLayers(1 << 9))
            return;
        //flashIndicator.Indicate();
        HP--;

        if (HP <= 0)
            Destroy(this.gameObject);
    }
}
