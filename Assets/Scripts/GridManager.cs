using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    [SerializeField] private float _spacing = 1.1f; // Cube同士の間隔
    [SerializeField] private TerrainCube _terrainCubePrefab;
    [SerializeField] private TerrainManager _terrainManager;
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
                _board[i, j]._terrainManager = _terrainManager;
                _board[i, j].TerrainType = TerrainType.VacantLot;
            }
        }
    }

    public void ChangeTerrainType(TerrainType type, (int,int) gridPos)
    {
        _board[gridPos.Item1, gridPos.Item2].TerrainType = type;
    }

    private void TopographicalChanges((int, int) gridPos, TerrainType type)
    {
        switch(type)
        {

        }
    }

    private bool FindTypeInAroundTerrain((int,int) gridPos ,TerrainType type)
    {
        (int, int)[] aroundGrid = new (int, int)[4]
        {
            (gridPos.Item1,gridPos.Item2 - 1),
            (gridPos.Item1,gridPos.Item2 + 1),
            (gridPos.Item1 - 1,gridPos.Item2),
            (gridPos.Item1 + 1,gridPos.Item2),
        };
        for(int i = 0; i < aroundGrid.Length; i++)
        {
            if(_board[aroundGrid[i].Item1, aroundGrid[i].Item2].TerrainType == type)
            {
                return true;
            }
        }
        return false;
    }

    public bool TryWorldPositionToGridPosition(Vector3 worldPos,out (int,int) gridPos)
    {
        for(int r = 0; r < _row; r++)
        {
            for(int c = 0; c < _column; c++)
            {
                if (_board[r,c].transform.position == worldPos)
                {
                    gridPos = (r,c);
                    return true;
                }
            }
        }
        gridPos = (-1, -1);
        return false;
    }
}
