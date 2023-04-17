using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IPuzzle
{
    void Interact();

    void GetPlayer(int value);

    int GetRoom(int value)
    {
        return value;
    }

    bool IsDone(bool value)
    {
        return value;
    }
}
