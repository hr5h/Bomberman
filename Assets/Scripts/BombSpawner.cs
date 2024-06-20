using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject bombPrefab;
    public float spawnCooldown = 2f;
    private bool canSpawn = true;
    private float timer = 0;
    void Update()
    {
        if (timer>0)
        {
            timer -= Time.deltaTime;
            if (timer<=0)
            {
                timer = 0;
                canSpawn = true;
            }
        }
        if (Input.GetKey(KeyCode.Space) && canSpawn)
        {
            SpawnBomb();
        }
    }
    void SpawnBomb()
    {
        canSpawn = false;
        timer = spawnCooldown;
        Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(transform.position.x / 4) * 4, 0, Mathf.RoundToInt(transform.position.z / 4) * 4), Quaternion.identity);
    }
}
