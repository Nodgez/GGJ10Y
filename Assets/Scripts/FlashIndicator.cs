using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashIndicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Texture mainTex;

    private Coroutine indicatorRoutine;
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void Indicate()
    {
        if (indicatorRoutine != null)
            StopCoroutine(indicatorRoutine);
        StartCoroutine(CO_Indicate());
    }

    IEnumerator CO_Indicate()
    {
        float t = 0;

        while (t < 1)
        {
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, t);
            t = Mathf.Clamp01(t + Time.deltaTime * 10);
            yield return null;
        }
    }
}
