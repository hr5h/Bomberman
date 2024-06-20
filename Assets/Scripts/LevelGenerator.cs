using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(LevelGenerator))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelGenerator level = (LevelGenerator)target;

        if (GUILayout.Button("Сгенерировать стены"))
        {
            level.GenerateWalls();
        }
        if (GUILayout.Button("Сгенерировать препятствия"))
        {
            level.GenerateBricks();
        }
    }
}
#endif

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject brickPrefab;
    public int levelSize = 15;
    public int bricksCount = 30;

    private GameObject wallsObject;
    private GameObject bricksObject;

    void Start()
    {
        GenerateBricks();
    }

    public void RespawnBricks()
    {
        int childCount = bricksObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            bricksObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void GenerateWalls()
    {
        Transform existingWalls = transform.Find("Walls");
        if (existingWalls != null)
        {
            DestroyImmediate(existingWalls.gameObject);
        }

        wallsObject = new GameObject("Walls");
        wallsObject.transform.parent = transform;

        GenerateExternalWalls();
        GenerateInternalWalls();
    }
    public void GenerateBricks()
    {
        Transform existingBricks = transform.Find("Bricks");
        if (existingBricks != null)
        {
            DestroyImmediate(existingBricks.gameObject);
        }

        bricksObject = new GameObject("Bricks");
        bricksObject.transform.parent = transform;

        List<(int, int)> availableCells = new List<(int, int)>();
        for (int x = 1; x < levelSize -1; x++)
        {
            for (int z = 1; z < levelSize - 1; z++)
            {
                if ((x%2 != 0 || z%2 != 0) && 
                    (x+z > 4) &&
                    (x+z < 24))
                {
                    availableCells.Add((z, x));
                }
            }
        }
        for (int i = 0; i < bricksCount; i++)
        {
            var index = Random.Range(0, availableCells.Count);
            InstantiateBrick(new Vector3(availableCells[index].Item2, 0, -availableCells[index].Item1) * 4);
            availableCells.RemoveAt(index);
        }
    }
    private void GenerateExternalWalls()
    {
        for (int x = 0; x < levelSize; x++)
        {
            InstantiateWall(new Vector3(x, 0, 0) * 4);
            InstantiateWall(new Vector3(x, 0, -(levelSize - 1)) * 4);
        }

        for (int z = 1; z < levelSize - 1; z++)
        {
            InstantiateWall(new Vector3(0, 0, -z) * 4);
            InstantiateWall(new Vector3(levelSize - 1, 0, -z) * 4);
        }
    }

    private void GenerateInternalWalls()
    {
        for (int x = 1; x < levelSize -1; x++)
        {
            for (int z = 1; z < levelSize - 1; z++)
            {
                if (x%2 == 0 && z%2 == 0)
                {
                    InstantiateWall(new Vector3(x, 0, -z) * 4);
                }
            }
        }
    }

    private void InstantiateWall(Vector3 position)
    {
        GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity);
        wall.transform.parent = wallsObject.transform;
    }

    private void InstantiateBrick(Vector3 position)
    {
        GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity);
        brick.transform.parent = bricksObject.transform;
    }
}