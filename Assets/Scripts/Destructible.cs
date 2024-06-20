using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour
{
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}
