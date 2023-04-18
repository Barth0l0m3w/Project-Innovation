using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPickInterface : MonoBehaviour, IPuzzle
{
    private int _player;
    public void Interact()
    {
        if (_player == 1)
        {
            
        }
    }

    public void GetPlayer(int value)
    {
        _player = value;
    }
}
