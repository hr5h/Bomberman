using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Win();
        }
    }
    private void Win()
    {
        EventManager.Instance.OnPlayerWin.Invoke();
    }
}
