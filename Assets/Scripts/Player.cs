using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destructible
{
    private PlayerMovement movement;
    private BombSpawner bombSpawner;
    private Vector3 spawnPoint;
    private void Awake()
    {
        spawnPoint = transform.position;
        movement = GetComponent<PlayerMovement>();
        bombSpawner = GetComponent<BombSpawner>();
    }
    public void Respawn()
    {
        EnableControl();
        transform.position = spawnPoint;
        gameObject.SetActive(true);
    }
    public void DisableControl()
    {
        movement.enabled = false;
        bombSpawner.enabled = false;
    }
    public void EnableControl()
    {
        movement.enabled = true;
        bombSpawner.enabled = true;
    }
    public override void Death()
    {
        EventManager.Instance.OnPlayerDead?.Invoke();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Death();
        }
    }
}
