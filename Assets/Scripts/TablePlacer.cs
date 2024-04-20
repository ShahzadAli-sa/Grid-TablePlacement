using UnityEngine;

namespace GridSystem
{
    public class TablePlacer : MonoBehaviour
    {
        private GameObject _tableHorizontalPrefab;
        private GameObject _tableVerticalPrefab;

        public TablePlacer(GameObject tableHorizontalPrefab, GameObject tableVerticalPrefab)
        {
            this._tableHorizontalPrefab = tableHorizontalPrefab;
            this._tableVerticalPrefab = tableVerticalPrefab;
        }

        public void PlaceTable(Vector2Int gridPosition, TileType[,] terrainGrid, bool[,] tileOccupiedStatus, string direction)
        {
       
            Vector3 tablePosition = new Vector3(gridPosition.x, gridPosition.y, 0); // Default position
            switch (direction)
            {
                case "Right":
                    Instantiate(_tableHorizontalPrefab, tablePosition, Quaternion.identity);
                    // Set the tiles as occupied by the table
                    tileOccupiedStatus[gridPosition.x, gridPosition.y] = true;
                    tileOccupiedStatus[gridPosition.x + 1, gridPosition.y] = true;
                    break;
                case "Left":
                    tablePosition.x -= 1;
                    Instantiate(_tableHorizontalPrefab, tablePosition, Quaternion.identity);
                    // Set the tiles as occupied by the table
                    tileOccupiedStatus[gridPosition.x, gridPosition.y] = true;
                    tileOccupiedStatus[gridPosition.x - 1, gridPosition.y] = true;
                    break;
                case "Up":
                    Instantiate(_tableVerticalPrefab, tablePosition, Quaternion.identity);
                    // Set the tiles as occupied by the table
                    tileOccupiedStatus[gridPosition.x, gridPosition.y] = true;
                    tileOccupiedStatus[gridPosition.x, gridPosition.y + 1] = true;
                    break;
                case "Down":
                    tablePosition.y -= 1;
                    Instantiate(_tableVerticalPrefab, tablePosition, Quaternion.identity);
                    // Set the tiles as occupied by the table
                    tileOccupiedStatus[gridPosition.x, gridPosition.y] = true;
                    tileOccupiedStatus[gridPosition.x, gridPosition.y - 1] = true;
                    break;
            }
        }
    }
}
