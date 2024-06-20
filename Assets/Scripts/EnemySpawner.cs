using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnLimit = 5;
    public float spawnCooldown = 5f;
    private bool canSpawn = false;
    private float timer;
    private List<Enemy> enemies = new List<Enemy>();
    private bool pause = false;

    private void Start()
    {
        timer = spawnCooldown;
        EventManager.Instance.OnLevelStarted.AddListener(DestroyAllEnemies);
        EventManager.Instance.OnLevelStarted.AddListener(ResetPause);
        EventManager.Instance.OnPlayerWin.AddListener(DestroyAllEnemies);
        EventManager.Instance.OnPlayerWin.AddListener(SetPause);
        EventManager.Instance.OnPlayerDead.AddListener(SetPause);
    }
    private void Update()
    {
        if (pause) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                canSpawn = true;
            }
        }
        if (enemies.Count < spawnLimit && canSpawn)
        {
            SpawnEnemy();
        }
    }
    private void SetPause()
    {
        pause = true;
    }
    private void ResetPause()
    {
        timer = spawnCooldown;
        pause = false;
    }
    private void SpawnEnemy()
    {
        canSpawn = false;
        timer = spawnCooldown;
        var enemy = Instantiate(enemyPrefab, transform.position, transform.rotation).GetComponent<Enemy>();
        enemies.Add(enemy);
        enemy.OnDeath.AddListener(OnEnemyDeath);
    }
    private void DestroyAllEnemies()
    {
        foreach(var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }
    private void OnEnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
