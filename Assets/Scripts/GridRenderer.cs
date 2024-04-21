using UnityEngine;

namespace GridSystem
{
    public class GridRenderer : MonoBehaviour
    {
        
        public GameObject[] TilePrefabs; // Array of tile prefabs (Dirt, Grass, Stone, Wood)
        public bool[,] TileOccupiedStatus; // Track the occupied status of each tile

        public void RenderGrid(TileType[,] terrainGrid)
        {
            for (int i = 0; i < terrainGrid.GetLength(0); i++)
            {
                for (int j = 0; j < terrainGrid.GetLength(1); j++)
                {
                    Vector3 position = GridToWorldPosition(i, j);
                    GameObject tilePrefab = TilePrefabs[(int)terrainGrid[i, j]];
                    Instantiate(tilePrefab, position, Quaternion.identity);
                }
            }
            InitializeTileOccupiedStatus(terrainGrid);

        }

        private Vector3 GridToWorldPosition(int x, int y)
        {
            // Convert grid coordinates to world position
            return new Vector3(x, y, 0);
        }

        private void InitializeTileOccupiedStatus(TileType[,] terrainGrid)
        {
            TileOccupiedStatus = new bool[terrainGrid.GetLength(0), terrainGrid.GetLength(1)];
            // Initialize all tiles as unoccupied initially
            for (int i = 0; i < terrainGrid.GetLength(0); i++)
            {
                for (int j = 0; j < terrainGrid.GetLength(1); j++)
                {
                    TileOccupiedStatus[i, j] = false;
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
}
