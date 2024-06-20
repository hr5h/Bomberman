using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public UnityEvent OnBombExploded;
    public UnityEvent OnPlayerDead;
    public UnityEvent OnPlayerWin;
    public UnityEvent OnBrickDestroy;
    public UnityEvent OnPlayerMoved;
    public UnityEvent OnLevelStarted;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
