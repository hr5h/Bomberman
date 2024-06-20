using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform follow;

    private void Update()
    {
        if (follow != null)
        {
            transform.position = follow.position;
        }
    }
}
