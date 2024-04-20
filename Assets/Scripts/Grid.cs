using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public string jsonFilePath = "Assets/Resources/data.json"; // Path to your JSON file
    public GameObject[] tilePrefabs; // Array of tile prefabs (Dirt, Grass, Stone, Wood)
    public GameObject tableHorizontalPrefab;
    public GameObject tableVerticalPrefab;

    private TileType[,] terrainGrid;
    private bool[,] tileOccupiedStatus; // Track the occupied status of each tile


    // public List<Tile> tilesList;
    string tableDirection;

    private void Start()
    {
        GridLoader(jsonFilePath);
        RenderGrid();
        InitializeTileOccupiedStatus();

    }

    private void GridLoader(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        GridData gridData = JsonConvert.DeserializeObject<GridData>(jsonString);
        terrainGrid = new TileType[gridData.TerrainGrid.GetLength(0), gridData.TerrainGrid.GetLength(1)];

        for (int i = 0; i < gridData.TerrainGrid.GetLength(0); i++)
        {
            for (int j = 0; j < gridData.TerrainGrid.GetLength(1); j++)
            {
                terrainGrid[i, j] = (TileType)gridData.TerrainGrid[i, j].TileType;
            }
        }
    }

    private void RenderGrid()
    {
        for (int i = 0; i < terrainGrid.GetLength(0); i++)
        {
            for (int j = 0; j < terrainGrid.GetLength(1); j++)
            {
                Vector3 position = GridToWorldPosition(i, j);
                GameObject tilePrefab = tilePrefabs[(int)terrainGrid[i, j]];
                Instantiate(tilePrefab, position, Quaternion.identity);
                //if (terrainGrid[i, j] == TileType.Wood)
                //{
                //    tilesList.Add(tilePrefab.GetComponent<Tile>());
                //}
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 clickPosition = hit.transform.position;
                Vector2Int gridPosition = ConvertToGridPosition(clickPosition);
                if (IsValidWoodTile(gridPosition) && HasSpaceForTable(gridPosition))
                {
                    PlaceTable(gridPosition);
                }
                
            }

        }

    }

    private bool IsValidWoodTile(Vector2Int gridPosition)
    {
        return terrainGrid[gridPosition.x, gridPosition.y] == TileType.Wood;
    }

    private bool HasSpaceForTable(Vector2Int gridPosition)
    {
        // Check if the clicked tile is of the 'Wood' type
        if (terrainGrid[gridPosition.x, gridPosition.y] != TileType.Wood)
        {
            return false; // Table can only be placed on tiles of the 'Wood' type
        }

        // Check if there is enough space horizontally for a table
        if (gridPosition.x + 1 < terrainGrid.GetLength(0) &&
            terrainGrid[gridPosition.x + 1, gridPosition.y] == TileType.Wood)
        {
            // Check if the position is not occupied by another table
            if (!IsPositionOccupiedByTable(gridPosition) &&
                !IsPositionOccupiedByTable(new Vector2Int(gridPosition.x + 1, gridPosition.y)))
            {
                tableDirection = "Right";
                return true; // Horizontal table can be placed here
            }
            else
            {
                Debug.Log("Tiles are occupied");
            }
        }

        // Check if there is enough space horizontally for a table
        if (gridPosition.x > 0 &&
            terrainGrid[gridPosition.x - 1, gridPosition.y] == TileType.Wood)
        {
            // Check if the position is not occupied by another table
            if (!IsPositionOccupiedByTable(gridPosition) &&
                !IsPositionOccupiedByTable(new Vector2Int(gridPosition.x - 1, gridPosition.y)))
            {
                tableDirection = "Left";
                return true; // Horizontal table can be placed here
            }
            else
            {
                Debug.Log("Tiles are occupied");
            }
        }

        // Check if there is enough space vertically for a table
        if (gridPosition.y + 1 < terrainGrid.GetLength(1) &&
            terrainGrid[gridPosition.x, gridPosition.y + 1] == TileType.Wood)
        {
            // Check if the position is not occupied by another table
            if (!IsPositionOccupiedByTable(gridPosition) &&
                !IsPositionOccupiedByTable(new Vector2Int(gridPosition.x, gridPosition.y + 1)))
            {
                tableDirection = "Up";
                return true; // Vertical table can be placed here
            }
            else
            {
                Debug.Log("Tiles are occupied");
            }
        }

        // Check if there is enough space vertically for a table
        if (gridPosition.y > 0 &&
            terrainGrid[gridPosition.x, gridPosition.y - 1] == TileType.Wood)
        {
            // Check if the position is not occupied by another table
            if (!IsPositionOccupiedByTable(gridPosition) &&
                !IsPositionOccupiedByTable(new Vector2Int(gridPosition.x, gridPosition.y - 1)))
            {
                tableDirection = "Down";
                return true; // Vertical table can be placed here
            }
            else
            {
                Debug.Log("Tiles are occupied");
            }
        }

        return false; // Insufficient space or position occupied by another table
    }

 

private bool IsPositionOccupiedByTable(Vector2 tilePosition)
    {

       return tileOccupiedStatus[(int)tilePosition.x, (int)tilePosition.y];

    }

    private void PlaceTable(Vector2Int gridPosition)
    {
        
        Vector3 tablePosition = GridToWorldPosition(gridPosition.x, gridPosition.y);
        GameObject newTable;
        tileOccupiedStatus[gridPosition.x, gridPosition.y] = true;

        Debug.Log("tableDirection...."+tableDirection);

        switch (tableDirection)
        {
            case "Right":
                 newTable = Instantiate(tableHorizontalPrefab, tablePosition, Quaternion.identity);// Set the tiles as occupied by the table
                tileOccupiedStatus[gridPosition.x + 1, gridPosition.y] = true; // Assuming the table is placed horizontally
                break;
            case "Left":
                tablePosition.x -= 1;
                 newTable = Instantiate(tableHorizontalPrefab, tablePosition, Quaternion.identity);
                tileOccupiedStatus[gridPosition.x - 1, gridPosition.y] = true; // Assuming the table is placed horizontally

                break;
            case "Up":
                 newTable = Instantiate(tableVerticalPrefab, tablePosition, Quaternion.identity);
                tileOccupiedStatus[gridPosition.x, gridPosition.y+1] = true; // Assuming the table is placed vertically

                break;
            case "Down":
                tablePosition.y -= 1;
                 newTable = Instantiate(tableVerticalPrefab, tablePosition, Quaternion.identity);
                tileOccupiedStatus[gridPosition.x, gridPosition.y - 1] = true; // Assuming the table is placed vertically

                break;

    }
        
    }

    private Vector2Int ConvertToGridPosition(Vector3 position)
    {
        // Convert world position to grid coordinates
        // You need to implement this based on your isometric grid layout
        // This is just a placeholder implementation
        return new Vector2Int((int)position.x, (int)position.y);
    }

    private void InitializeTileOccupiedStatus()
    {
        tileOccupiedStatus = new bool[terrainGrid.GetLength(0), terrainGrid.GetLength(1)];
        // Initialize all tiles as unoccupied initially
        for (int i = 0; i < terrainGrid.GetLength(0); i++)
        {
            for (int j = 0; j < terrainGrid.GetLength(1); j++)
            {
                tileOccupiedStatus[i, j] = false;
            }
        }
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
public class GridData
{
    public GridCellData[,] TerrainGrid;
}

[System.Serializable]
public class GridCellData
{
    public int TileType;
}
