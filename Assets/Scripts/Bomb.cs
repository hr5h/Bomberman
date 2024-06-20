using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float lifeTime = 2f;
    public int explosionRange = 3;

    private void Start()
    {
        EventManager.Instance.OnPlayerDead.AddListener(Clear);
        EventManager.Instance.OnPlayerWin.AddListener(Clear);
    }
    private void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }
    private void Clear()
    {
        Destroy(gameObject);
    }
    private void Explode()
    {
        EventManager.Instance.OnBombExploded?.Invoke();
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        for (int direction = 0; direction < 4; ++direction)
        {
            for (int i = 0; i < explosionRange; ++i)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position + GetDirectionVector(direction) * 4 * (i + 1), 1f);
                bool collisionWithWall = false;
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.tag == "Wall")
                    {
                        collisionWithWall = true;
                        break;
                    }
                    if (collider.gameObject.tag == "Brick")
                    {
                        collisionWithWall = true;
                        Instantiate(explosionPrefab, transform.position + GetDirectionVector(direction) * 4 * (i + 1), Quaternion.identity);
                        break;
                    }
                }
                if (collisionWithWall)
                {
                    break;
                }
                Instantiate(explosionPrefab, transform.position + GetDirectionVector(direction) * 4 * (i + 1), Quaternion.identity);
            }
        }
    }

    private Vector3 GetDirectionVector(int direction)
    {
        switch (direction)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.right;
            case 2:
                return Vector3.back;
            case 3:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }
}
