using UnityEngine;
using UnityEngine.SceneManagement;

namespace GridSystem
{
    public class GridInteraction : MonoBehaviour
    {
        public GridLoader GridLoader;
        public GridRenderer GridRenderer;
        public GameObject TableHorizontalPrefab;
        public GameObject TableVerticalPrefab;

        private TablePlacementValidator _placementValidator;
        private TablePlacer _tablePlacer;

        TileType[,] _terrainGrid;

        private void Start()
        {
            GridLoader.OnGridLoaded += HandleGridLoaded;
            _placementValidator = new TablePlacementValidator();
            _tablePlacer = new TablePlacer(TableHorizontalPrefab, TableVerticalPrefab);
            GridLoader.LoadGridData();
        }

        private void HandleGridLoaded(TileType[,] terrainGrid)
        {
            this._terrainGrid = terrainGrid;
            GridRenderer.RenderGrid(terrainGrid);
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
                    if (_placementValidator.IsValidWoodTile(_terrainGrid[gridPosition.x, gridPosition.y]) &&
                        _placementValidator.HasSpaceForTable(gridPosition, _terrainGrid, GridRenderer.TileOccupiedStatus, out string direction))
                    {
                        _tablePlacer.PlaceTable(gridPosition,_terrainGrid,GridRenderer.TileOccupiedStatus,direction);
                    }

                }

            }
        }

        private Vector2Int ConvertToGridPosition(Vector3 position)
        {
            // Convert world position to grid coordinates
            return new Vector2Int((int)position.x, (int)position.y);
        }

        public void RestartLevel()
        {
            // Get the name of the currently active scene
            string sceneName = SceneManager.GetActiveScene().name;

            // Reload the scene by its name
            SceneManager.LoadScene(sceneName);
        }

    }


}
