using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/Resources/data.json"; // Path to your JSON file
    public GameObject[] tilePrefabs; // Array of tile prefabs (Dirt, Grass, Stone, Wood)
    public GameObject tablePrefab;

    private TileType[,] terrainGrid;


    private void Start()
    {
        
        LoadLevel(jsonFilePath);
        RenderLevel();
    }

    private void LoadLevel(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonString);
        terrainGrid = new TileType[levelData.TerrainGrid.GetLength(0), levelData.TerrainGrid.GetLength(1)];

        for (int i = 0; i < levelData.TerrainGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelData.TerrainGrid.GetLength(1); j++)
            {
                terrainGrid[i, j] = (TileType)levelData.TerrainGrid[i, j].TileType;
            }
        }
    }

    private void RenderLevel()
    {
        for (int i = 0; i < terrainGrid.GetLength(0); i++)
        {
            for (int j = 0; j < terrainGrid.GetLength(1); j++)
            {
                Vector3 position = GridToWorldPosition(i, j);
                GameObject tilePrefab = tilePrefabs[(int)terrainGrid[i, j]];
                Instantiate(tilePrefab, position, Quaternion.identity);
            }
        }
    }

    private Vector3 GridToWorldPosition(int x, int y)
    {
        
        // Convert grid coordinates to world position
        // You need to implement this based on your isometric grid layout
        // This is just a placeholder implementation
        return new Vector3(x, y, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log($"clickPosition {clickPosition}");
            Vector2Int gridPosition = ConvertToGridPosition(clickPosition);
            Debug.Log($"Click gridPosition {gridPosition}");
            if (IsValidWoodTile(gridPosition) && HasSpaceForTable(gridPosition))
            {
                PlaceTable(gridPosition);
            }
        }
    }

    private bool IsValidWoodTile(Vector2Int gridPosition)
    {
        return terrainGrid[gridPosition.x, gridPosition.y] == TileType.Wood;
    }

    private bool HasSpaceForTable(Vector2Int gridPosition)
    {
        // Check if there is enough space for the table at gridPosition
        // You need to implement this based on your grid layout
        return true; // Placeholder return value
    }

    private void PlaceTable(Vector2Int gridPosition)
    {
        Vector3 tablePosition = GridToWorldPosition(gridPosition.x, gridPosition.y);
        GameObject newTable = Instantiate(tablePrefab, tablePosition, Quaternion.identity);
        // Optionally, you can store placed tables in a list for further manipulation
    }

    private Vector2Int ConvertToGridPosition(Vector3 position)
    {
        // Convert world position to grid coordinates
        // You need to implement this based on your isometric grid layout
        // This is just a placeholder implementation
        return new Vector2Int((int)position.x, (int)position.y);
    }
}

public enum TileType
{
    Dirt = 0,
    Grass = 1,
    Stone = 2,
    Wood = 3
}

[System.Serializable]
public class LevelData
{
    public GridCellData[,] TerrainGrid;
}

[System.Serializable]
public class GridCellData
{
    public int TileType;
}
