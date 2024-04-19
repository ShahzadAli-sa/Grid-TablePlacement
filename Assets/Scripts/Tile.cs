using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{

    // check if tile is occupied by table.
    public bool isOccupied;

    private void Start()
    {
        isOccupied = false;
    }

    // Define the enum TileType
    public enum TileType
    {
        Dirt = 0,
        Grass = 1,
        Stone = 2,
        Wood = 3
    }

    // Expose the TileType enum as a serialized field
    [SerializeField]
    public TileType tileType;
}
