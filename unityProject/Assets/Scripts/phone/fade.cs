using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade : MonoBehaviour
{
    public Material material;
    public float startOpacity = 1f;
    public float endOpacity = 0f;
    public float fadeTime = 2f;

    private void Start()
    {
        material.color = new Color(1f, 1f, 1f, startOpacity);
        StartCoroutine(FadeMaterialOpacityCoroutine());
    }

    private IEnumerator FadeMaterialOpacityCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / fadeTime);
            float currentOpacity = Mathf.Lerp(startOpacity, endOpacity, t);

            Color color = material.color;
            color.a = currentOpacity;
            material.color = color;

            yield return null;
        }
    }
}
