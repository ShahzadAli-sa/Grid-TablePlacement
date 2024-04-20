using UnityEngine;

namespace GridSystem
{
    public class TablePlacementValidator
    {
        public bool IsValidWoodTile(TileType tileType)
        {
            return tileType == TileType.Wood;
        }

        public bool HasSpaceForTable(Vector2Int gridPosition, TileType[,] terrainGrid, bool[,] tileOccupiedStatus, out string direction)
        {
            direction = "";

            // Check if the clicked tile is of the 'Wood' type
            if (!IsValidWoodTile(terrainGrid[gridPosition.x, gridPosition.y]))
                return false;

            // Check if there is enough space horizontally for a table
            if (gridPosition.x + 1 < terrainGrid.GetLength(0) &&
                IsValidWoodTile(terrainGrid[gridPosition.x + 1, gridPosition.y]))
            {
                if (!IsPositionOccupiedByTable(gridPosition.x, gridPosition.y, tileOccupiedStatus) &&
                    !IsPositionOccupiedByTable(gridPosition.x + 1, gridPosition.y, tileOccupiedStatus))
                {
                    direction = "Right";
                    return true; // Horizontal table can be placed here
                }
            }

            if (gridPosition.x > 0 &&
                IsValidWoodTile(terrainGrid[gridPosition.x - 1, gridPosition.y]))
            {
                if (!IsPositionOccupiedByTable(gridPosition.x, gridPosition.y, tileOccupiedStatus) &&
                    !IsPositionOccupiedByTable(gridPosition.x - 1, gridPosition.y, tileOccupiedStatus))
                {
                    direction = "Left";
                    return true; // Horizontal table can be placed here
                }
            }

            // Check if there is enough space vertically for a table
            if (gridPosition.y + 1 < terrainGrid.GetLength(1) &&
                IsValidWoodTile(terrainGrid[gridPosition.x, gridPosition.y + 1]))
            {
                if (!IsPositionOccupiedByTable(gridPosition.x, gridPosition.y, tileOccupiedStatus) &&
                    !IsPositionOccupiedByTable(gridPosition.x, gridPosition.y + 1, tileOccupiedStatus))
                {
                    direction = "Up";
                    return true; // Vertical table can be placed here
                }
            }

            if (gridPosition.y > 0 &&
                IsValidWoodTile(terrainGrid[gridPosition.x, gridPosition.y - 1]))
            {
                if (!IsPositionOccupiedByTable(gridPosition.x, gridPosition.y, tileOccupiedStatus) &&
                    !IsPositionOccupiedByTable(gridPosition.x, gridPosition.y - 1, tileOccupiedStatus))
                {
                    direction = "Down";
                    return true; // Vertical table can be placed here
                }
            }

            return false; // Insufficient space or position occupied by another table
        }

        private bool IsPositionOccupiedByTable(int x, int y, bool[,] tileOccupiedStatus)
        {
            // Check if the position is within the bounds of the grid
            if (x < 0 || x >= tileOccupiedStatus.GetLength(0) || y < 0 || y >= tileOccupiedStatus.GetLength(1))
                return true; // Position is out of bounds, consider it occupied

            // Check if the position is occupied by a table
            return tileOccupiedStatus[x, y];
        }
    }
}
