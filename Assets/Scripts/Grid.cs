using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Grid : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Prefabs for different tile types
    public GameObject tablePrefab;   // Prefab for the table
    public string levelFilePath;     // Path to the JSON level file

    private int[,] terrainGrid;      // 2D array to store the tile types

    void Start()
    {
        LoadLevelFromJson();
        RenderLevel();
    }

    // Load level data from JSON file
    private void LoadLevelFromJson()
    {
        string json = File.ReadAllText(levelFilePath);
        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(json);
        terrainGrid = levelData.TerrainGrid;
    }

    // Render the level based on the tile types
    private void RenderLevel()
    {
        for (int x = 0; x < terrainGrid.GetLength(0); x++)
        {
            for (int y = 0; y < terrainGrid.GetLength(1); y++)
            {
                int tileType = terrainGrid[x, y];
                GameObject tilePrefab = tilePrefabs[tileType];
                Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }

    // Handle mouse click for table placement
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                int gridX = Mathf.FloorToInt(hit.point.x);
                int gridZ = Mathf.FloorToInt(hit.point.z);

                if (terrainGrid[gridX, gridZ] == 4 && CanPlaceTable(gridX, gridZ))
                {
                    PlaceTable(gridX, gridZ);
                }
            }
        }
    }

    // Check if the table can be placed at the given grid coordinates
    private bool CanPlaceTable(int x, int z)
    {
        if (x + 1 < terrainGrid.GetLength(0) && terrainGrid[x + 1, z] == 4)
        {
            return true; // Horizontal placement
        }
        else if (z + 1 < terrainGrid.GetLength(1) && terrainGrid[x, z + 1] == 4)
        {
            return true; // Vertical placement
        }
        return false;
    }

    // Place the table at the given grid coordinates
    private void PlaceTable(int x, int z)
    {
        GameObject newTable = Instantiate(tablePrefab, new Vector3(x + 0.5f, 0, z + 0.5f), Quaternion.identity);
        if (x + 1 < terrainGrid.GetLength(0) && terrainGrid[x + 1, z] == 4)
        {
            newTable.transform.rotation = Quaternion.Euler(0, 90, 0); // Horizontal placement
        }
    }
}

// Class to deserialize JSON level data
[System.Serializable]
public class LevelData
{
    public int[,] TerrainGrid;
}
