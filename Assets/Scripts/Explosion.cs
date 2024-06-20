using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionDuration = 0.2f;
    private Collider explosionArea;
    private void Start()
    {
        explosionArea = GetComponent<Collider>();
    }
    private void Update()
    {
        if (explosionDuration > 0)
        {
            explosionDuration -= Time.deltaTime;
            if (explosionDuration <= 0)
            {
                explosionArea.enabled = false;
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        Destructible destructible;
        if (other.TryGetComponent(out destructible))
        {
            destructible.Death();
        }
    }
}
