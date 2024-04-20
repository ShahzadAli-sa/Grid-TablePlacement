using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace GridSystem
{
    public class GridLoader : MonoBehaviour
    {
        public string jsonFilePath = "Assets/Resources/data.json"; // Path to your JSON file

        public event Action<TileType[,]> OnGridLoaded; // Event to notify when grid data is loaded
       
        TileType[,] terrainGrid;
        public void LoadGridData()
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            GridData gridData = JsonConvert.DeserializeObject<GridData>(jsonString);
            terrainGrid = new TileType[gridData.TerrainGrid.GetLength(0), gridData.TerrainGrid.GetLength(1)];

            for (int i = 0; i < gridData.TerrainGrid.GetLength(0); i++)
            {
                for (int j = 0; j < gridData.TerrainGrid.GetLength(1); j++)
                {
                    terrainGrid[i, j] = (TileType)gridData.TerrainGrid[i, j].TileType;
                }
            }
            OnGridLoaded?.Invoke(terrainGrid);
        }
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

}
