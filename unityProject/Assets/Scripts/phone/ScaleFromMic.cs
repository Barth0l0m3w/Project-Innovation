using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScaleFromMic : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public MicDetection detector;

    [SerializeField]
    private Material material;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    [SerializeField]
    private float blowSpeed = 20f;


    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMic() * loudnessSensibility;

        if (loudness < threshold)
        {
            loudness = 0;
        }
        if (loudness > threshold)
        {
            //To do Kama
            //StartCoroutine(Fade()) (fading dust) starts when loudness > threshold. it always detects. maybe put it in Fading() and call te when you activate it. 
            StartCoroutine(Fade());
        }

        if (material.GetFloat("_Fade") == 0)
        {
            Debug.Log("object completely cleaned");
        }
    }

    void Fading()
    {
        
    }

    IEnumerator Fade()
    {
        //slowly lessen the opacity through the shader
        float time = 1f;
        while (time > 0f)
        {
            time -= Time.deltaTime / blowSpeed;
            material.SetFloat("_Fade", time);
            yield return null;
        }
    }
}
