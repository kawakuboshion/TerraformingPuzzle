using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    [SerializeField] private float _spacing = 1.1f; // Cube同士の間隔
    [SerializeField] private TerrainCube _terrainCubePrefab;
    private TerrainCube[,] _board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _board = new TerrainCube[_row, _column];
        for(int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                _board[i, j] = Instantiate(_terrainCubePrefab, new Vector3(i * _spacing, 0, j * _spacing), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
