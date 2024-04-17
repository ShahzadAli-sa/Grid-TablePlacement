using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Prefabs for different tile types
    public GameObject tablePrefab;   // Prefab for the table
    public string levelFilePath;     // Path to the JSON level file

    private List<List<int>> terrainGrid; // List of lists to store the tile types

    void Start()
    {
        LoadLevelFromJson();
        RenderLevel();
    }

    // Load level data from JSON file
    private void LoadLevelFromJson()
    {
        string json = File.ReadAllText(levelFilePath);
        dynamic jsonData = JsonConvert.DeserializeObject(json);
        dynamic gridData = jsonData.TerrainGrid.ToObject<List<List<int>>>();//JsonConvert.DeserializeObject<List<List<int>>>(jsonData["TerrainGrid"].ToString());

        terrainGrid = gridData.ToObject<List<List<int>>>();
    }

    // Render the level based on the tile types
    private void RenderLevel()
    {
        for (int x = 0; x < terrainGrid.Count; x++)
        {
            for (int y = 0; y < terrainGrid[x].Count; y++)
            {
                int tileType = terrainGrid[x][y];
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

                if (IsValidTablePosition(gridX, gridZ))
                {
                    PlaceTable(gridX, gridZ);
                }
            }
        }
    }

    // Check if the table can be placed at the given grid coordinates
    private bool IsValidTablePosition(int x, int z)
    {
        if (x + 1 < terrainGrid.Count && terrainGrid[x + 1][z] == 4)
        {
            return true; // Horizontal placement
        }
        else if (z + 1 < terrainGrid[x].Count && terrainGrid[x][z + 1] == 4)
        {
            return true; // Vertical placement
        }
        return false;
    }

    // Place the table at the given grid coordinates
    private void PlaceTable(int x, int z)
    {
        GameObject newTable = Instantiate(tablePrefab, new Vector3(x + 0.5f, 0, z + 0.5f), Quaternion.identity);
        if (x + 1 < terrainGrid.Count && terrainGrid[x + 1][z] == 4)
        {
            newTable.transform.rotation = Quaternion.Euler(0, 90, 0); // Horizontal placement
        }
    }


}
