using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _row = 9;
    [SerializeField] private int _column = 16;
    [SerializeField] private float _tileSpacing = 0.5f;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform cam;

    private Dictionary<Vector2, Tile> _tiles;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();

        Tile spawnedTile;

        for (int x = 0; x < _row; x++)
        {
            for (int y = 0; y < _column; y++)
            {
                float posX = x * _tileSpacing;
                if (y % 2 == 0)
                {
                    posX += _tileSpacing / 2; // Offset every second row
                    spawnedTile = Instantiate(_tilePrefab, new Vector3(posX, y), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}"; // Naming the tile for easier identification
                }
                else
                {
                    posX -= _tileSpacing / 2;
                    spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}"; // Naming the tile for easier identification
                }

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        //cam.transform.position = new Vector3((float)_row / 2 - 0.5f, (float)_column / 2 - 0.5f, -10);
    }
}
