using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _rows = 7;
    [SerializeField] private int _columns = 12;
    [SerializeField] private float _hexSize = 1f;

    [Header("Grid Position")]
    [SerializeField] private Vector2 _gridOrigin = Vector2.zero; // Bottom-left corner of the grid
    
    [SerializeField] private Tile _tilePrefab;

    private Dictionary<Vector2Int, Tile> _tiles;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2Int, Tile>();

        // Calculate hex geometry
        float hexWidth = Mathf.Sqrt(3f) * _hexSize;
        float hexHeight = 2f * _hexSize;

        float xSpacing = hexWidth * 0.75f; // horizontal distance between hex centers
        float ySpacing = hexHeight * 0.5f; // vertical offset for odd columns

        for (int x = 0; x < _rows; x++)
        {
            for (int y = 0; y < _columns; y++)
            {
                // Base position from row/column
                float posX = _gridOrigin.x + x * xSpacing;
                float posY = _gridOrigin.y + y * hexHeight;

                // Offset odd columns down by half a hex
                if (x % 2 == 1)
                    posY += ySpacing;

                Vector3 tilePos = new Vector3(posX, posY, 0);

                Tile spawnedTile = Instantiate(_tilePrefab, tilePos, Quaternion.identity);
                spawnedTile.name = $"Hex {x},{y}";

                _tiles[new Vector2Int(x, y)] = spawnedTile;
            }
        }
    }
}
