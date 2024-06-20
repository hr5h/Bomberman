using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public LevelGenerator generator;
    public Player player;
    public Grid grid;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            grid = new Grid(generator.levelSize, generator.levelSize);
            AddWallsToObstaclesMap();
            Instance = this;
        }
    }
    private void Start()
    {
        if (Instance == this)
        {
            EventManager.Instance.OnPlayerDead.AddListener(RestartLevel);
            EventManager.Instance.OnPlayerWin.AddListener(Win);
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Quit();
        }
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Debug.Log("Obstacles Map");
            for (int i = 0; i < 15; i++)
            {
                string row = "";
                for (int j = 0; j < 15; j++)
                {
                    row += grid[i, j].IsObstacle ? "1 " : "0 ";
                }
                Debug.Log(row);
            }
        }
#endif
    }
    private void AddWallsToObstaclesMap()
    {
        var h = grid.Columns;
        var w = grid.Rows;

        for (int i = 0; i < grid.Columns; i++)
        {
            SetObstacle(i,0);
            SetObstacle(i, grid.Columns - 1);
        }
        for (int i = 1; i < grid.Rows - 1; i++)
        {
            SetObstacle(0, i);
            SetObstacle(grid.Rows - 1, i);
        }

        for (int i = 1; i < grid.Rows - 1; i++)
        {
            for (int j = 1; j < grid.Columns - 1; j++)
            {
                if (i % 2 == 0 && j % 2 == 0)
                {
                    SetObstacle(i,j);
                }
            }
        }
    }
    public void SetObstacle(int x, int y)
    {
        grid[y, x].SetObstacle();
    }
    public void ResetObstacle(int x, int y)
    {
        grid[y, x].ResetObstacle();
    }
    public (int, int) GetPlayerPos()
    {
        return (Mathf.RoundToInt(-player.transform.position.z / 4), Mathf.RoundToInt(player.transform.position.x / 4));
    }
    public void Win()
    {
        Debug.Log("Win");
        StartCoroutine(WinCoroutine());
    }
    IEnumerator WinCoroutine()
    {
        player.DisableControl();
        Overlay.Instance.ShowMessage("Уровень пройден!");
        yield return new WaitForSeconds(1f);
        EventManager.Instance.OnLevelStarted?.Invoke();
        generator.GenerateBricks();
        player.Respawn();
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelCoroutine());
    }
    IEnumerator RestartLevelCoroutine()
    {
        yield return new WaitForSeconds(1);
        EventManager.Instance.OnLevelStarted?.Invoke();
        player.Respawn();
        generator.RespawnBricks();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
