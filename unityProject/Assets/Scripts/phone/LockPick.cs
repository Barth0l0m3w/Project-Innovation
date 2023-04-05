using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class LockPick : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform InnerLock;
    [SerializeField]
    private Transform pickPosition;

    [SerializeField]
    private float maxAngle = 90;

    [SerializeField]
    private float lockSpeed = 10;

    [SerializeField]
    [Min(1)][Range(1, 25)] private float lockRange = 10;

    private float eulerAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    private float keyPressTime;

    private bool movePick = true;

    // Start is called before the first frame update 
    void Start()
    {
        newLock();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = pickPosition.position;

        if (movePick)
        {
            Vector3 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);

            eulerAngle = Vector3.Angle(dir, Vector3.up);

            Vector3 cross = Vector3.Cross(Vector3.up, dir);

            if (cross.z < 0)
            {
                eulerAngle = -eulerAngle;
            }

            eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

            Quaternion rotateT0 = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
            transform.rotation = rotateT0;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            movePick = false;
            keyPressTime = 1;

        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            movePick = true;
            keyPressTime = 0;
        }

        keyPressTime = Mathf.Clamp(keyPressTime, 0, 1);

        float percentage = Mathf.Round(100 - Mathf.Abs(((eulerAngle - unlockAngle) / 100) * 100));
        float lockRotation = ((percentage / 100) * maxAngle * keyPressTime);
        float maxRotation = (percentage / 100) * maxAngle;

        float lockLerp = Mathf.Lerp(InnerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        InnerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if(lockLerp>=maxRotation -1)
        {
            if(eulerAngle<unlockRange.y && eulerAngle> unlockRange.x)
            {
                Debug.Log("unlocked");
                newLock();

                movePick = true;
                keyPressTime = 0;
            }
            else
            {
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));
            }
        }
    }

    void newLock()
    {
        unlockAngle = Random.Range(-maxAngle + lockRange, maxAngle += -lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }
}
