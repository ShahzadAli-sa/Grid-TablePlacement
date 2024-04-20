using UnityEngine;

namespace GridSystem
{
    public class GridInteraction : MonoBehaviour
    {
        public GridLoader gridLoader;
        public GridRenderer gridRenderer;
        public GameObject tableHorizontalPrefab;
        public GameObject tableVerticalPrefab;

        private TablePlacementValidator placementValidator;
        private TablePlacer tablePlacer;

        TileType[,] terrainGrid;

        private void Start()
        {
            gridLoader.OnGridLoaded += HandleGridLoaded;
            placementValidator = new TablePlacementValidator();
            tablePlacer = new TablePlacer(tableHorizontalPrefab, tableVerticalPrefab);
            gridLoader.LoadGridData();
        }

        private void HandleGridLoaded(TileType[,] terrainGrid)
        {
            this.terrainGrid = terrainGrid;
            gridRenderer.RenderGrid(terrainGrid);
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
                    if (placementValidator.IsValidWoodTile(terrainGrid[gridPosition.x, gridPosition.y]) &&
                        placementValidator.HasSpaceForTable(gridPosition, terrainGrid, gridRenderer.tileOccupiedStatus, out string direction))
                    {
                        tablePlacer.PlaceTable(gridPosition,terrainGrid,gridRenderer.tileOccupiedStatus,direction);
                    }

                }

            }
        }

        private Vector2Int ConvertToGridPosition(Vector3 position)
        {
            // Convert world position to grid coordinates
            return new Vector2Int((int)position.x, (int)position.y);
        }

    }


}
