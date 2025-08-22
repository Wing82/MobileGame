using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Hex Grid Settings")]
    [SerializeField] private float _hexSize = 1f;  // Radius of hex (center to corner)

    [Header("Grid Area")]
    [SerializeField] private float _gridWidth = 10f;   // World units (X direction)
    [SerializeField] private float _gridHeight = 10f;  // World units (Y direction)
    [SerializeField] private Vector2 _gridOrigin = Vector2.zero; // Bottom-left corner of the grid

    [SerializeField] private Tile _tilePrefab;

    private Dictionary<Vector2Int, Tile> _tiles;

    private void Start()
    {
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        _tiles = new Dictionary<Vector2Int, Tile>();

        // Hex geometry
        float hexWidth = Mathf.Sqrt(3f) * _hexSize;
        float hexHeight = 2f * _hexSize;

        float xSpacing = hexWidth * 0.75f;
        float ySpacing = hexHeight * 0.5f;

        // Calculate how many hexes fit into the given area
        int maxCols = Mathf.FloorToInt(_gridWidth / xSpacing);
        int maxRows = Mathf.FloorToInt(_gridHeight / hexHeight);

        for (int x = 0; x < maxCols; x++)
        {
            for (int y = 0; y < maxRows; y++)
            {
                float posX = _gridOrigin.x + x * xSpacing;
                float posY = _gridOrigin.y + y * hexHeight;

                // Offset odd columns
                if (x % 2 == 1)
                    posY += ySpacing;

                // Don’t spawn outside defined rectangle
                if (posX > _gridOrigin.x + _gridWidth || posY > _gridOrigin.y + _gridHeight)
                    continue;

                Vector3 tilePos = new Vector3(posX, posY, 0);
                Tile spawnedTile = Instantiate(_tilePrefab, tilePos, Quaternion.identity);
                spawnedTile.name = $"Hex {x},{y}";

                _tiles[new Vector2Int(x, y)] = spawnedTile;
            }
        }
    }
}
