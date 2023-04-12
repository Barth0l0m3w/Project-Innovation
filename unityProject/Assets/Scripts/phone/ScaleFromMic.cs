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


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Fade());
    }

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
            StartCoroutine(Fade());
            //Fading();
        }

        if (material.GetFloat("_Fade") == 0)
        {
            Debug.Log("object completely cleaned");
        }

        //transform.localScale = Vector3.Lerp(maxScale, minScale, loudness);
    }

    void Fading()
    {
        
    }

    IEnumerator Fade()
    {
        float time = 1f;
        while (time > 0f)
        {
            time -= Time.deltaTime / blowSpeed;
            material.SetFloat("_Fade", time);
            //Debug.Log(time);
            yield return null;
        }
    }
}
