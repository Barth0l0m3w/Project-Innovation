using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgroundsound : MonoBehaviour
{
    public float sounddelay = 50f; 
    AudioSource backgroundSound;
    // Start is called before the first frame update
    void Start()
    {
        backgroundSound= GetComponent<AudioSource>();
        backgroundSound.PlayDelayed(sounddelay);
    }

    // Update is called once per frame
    void playAudio()
    {
        backgroundSound.Play();
    }
}
