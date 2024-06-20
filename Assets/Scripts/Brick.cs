using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Destructible
{
    private int x;
    private int z;
    private void Awake()
    {
        x = Mathf.RoundToInt(transform.position.x / 4);
        z = Mathf.RoundToInt(-transform.position.z / 4);
    }
    private void Start()
    {
        LevelManager.Instance?.SetObstacle(x, z);
    }
    private void OnEnable()
    {
        LevelManager.Instance?.SetObstacle(x, z);
    }
    private void OnDisable()
    {
        LevelManager.Instance?.ResetObstacle(x, z);
        EventManager.Instance.OnBrickDestroy?.Invoke();
    }
    private void OnDestroy()
    {
        LevelManager.Instance?.ResetObstacle(x, z);
        EventManager.Instance.OnBrickDestroy?.Invoke();
    }
    public override void Death()
    {
        gameObject.SetActive(false);
    }
}
