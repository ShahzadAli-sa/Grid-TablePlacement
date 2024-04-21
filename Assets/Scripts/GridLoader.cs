using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GridSystem
{
    public class GridLoader : MonoBehaviour
    {
        public TextAsset JsonFile;  // Json File
        private string _jsonFileText;

        public event Action<TileType[,]> OnGridLoaded; // Event to notify when grid data is loaded
       
        TileType[,] _terrainGrid;
        private void Start()
        {
            _jsonFileText = JsonFile.text;
        }
        public void LoadGridData()
        {
            GridDataList gridData = JsonConvert.DeserializeObject<GridDataList>(_jsonFileText);
            _terrainGrid = new TileType[gridData.Terraingrid.Count, gridData.Terraingrid[0].Count];
            
            for (int i = 0; i < gridData.Terraingrid.Count; i++)
            {
                for (int j = 0; j < gridData.Terraingrid[0].Count; j++)
                {
                    _terrainGrid[i, j] = (TileType)gridData.Terraingrid[i][j].TileType;
                }
            }
            OnGridLoaded?.Invoke(_terrainGrid);
        }
    }

    
    [System.Serializable]
    public class GridCellData
    {
        public int TileType;
    }

    public class GridDataList
    {
        public List<List<GridCellData>> Terraingrid { get; set; }
    }

  

}
