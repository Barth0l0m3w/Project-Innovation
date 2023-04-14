using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezePhone : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 rot;
    [SerializeField] float timer = 3f;
    [SerializeField] private float time;
    [SerializeField] private bool startFreeze = false;
    [SerializeField] private bool caught = false;
    [SerializeField] private float treshold = 5f;
    private Quaternion startRot;


    void Start()
    {
        rot = Vector3.zero;
        Input.gyro.enabled = true;

        startRot = this.transform.rotation;

        //To Do Kama.
        //i am really sorry everything is in update
        //for now this thing starts the interaction.
        startFreeze = true;
    }

    private void Update()
    {
        if (startFreeze == true)
        {
            rotate();
            time = Time.deltaTime;

            if (time > timer && caught == false)
            {
                time = 0;
                Debug.Log("you've survived, congrats");
                startFreeze = false;
            }
        }

        if (caught == true && startFreeze == true)
        {
            Debug.Log("caught motherfucker");
            //change scene here. or do something at least
        }
    }

    private void StartInteraction()
    {

    }

    void rotate()
    {
        rot = Input.gyro.rotationRateUnbiased;
        transform.Rotate(rot);

        float diffX;
        diffX = startRot.x - rot.x;
        float diffY;
        diffY = startRot.y - rot.y;
        float diffZ;
        diffZ = startRot.z - rot.z;

        if (diffX > treshold || diffY > treshold || diffZ > treshold)
        {
            caught = true;
        }
    }
}
