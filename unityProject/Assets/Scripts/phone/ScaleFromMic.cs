using System;
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

    [SerializeField] private Material material;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    [SerializeField] private float blowSpeed = 20f;

    private float _loudness = 0;

    private void OnEnable()
    {
        //Client.Instance.PuzzleSolved = false;
    }

    // Update is called once per frame
    void Update()
    {
        
            _loudness = detector.GetLoudnessFromMic() * loudnessSensibility;
        

        if (_loudness < threshold)
        {
            _loudness = 0;
        }

        if (_loudness > threshold)
        {
            //To do Kama
            //StartCoroutine(Fade()) (fading dust) starts when loudness > threshold. it always detects. maybe put it in Fading() and call te when you activate it. 
            StartCoroutine(Fade());
        }

        if (material.GetFloat("_Fade") == 0)
        {
            Debug.Log("object completely cleaned");
            // Client.Instance.BlowDust = true;
            // Client.Instance.PuzzleSolved = true;
        }
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
            yield return null;
        }
    }
}