using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour
{
    private int _layerIgnore;
    private int _layerDefault;
    private void Start()
    {
        _layerIgnore = LayerMask.NameToLayer("Camera1Ignore");
        _layerDefault = LayerMask.NameToLayer("Default");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("AAAAA" + other);

        other.gameObject.layer = _layerIgnore;
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.layer = _layerDefault;
    }
}
