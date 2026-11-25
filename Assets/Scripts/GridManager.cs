using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _row;
    [SerializeField] private int _column;
    [SerializeField] private float _spacing = 1.1f;
    [SerializeField] private TerrainCube _terrainCubePrefab;
    [SerializeField] private TerrainManager _terrainManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TerrainTransitionRules _terrainTransitionRules;
    [SerializeField] private EnvironmentalChangeHandler _environmentalChangeHandler;

    public int Row { get => _row; }
    public int Column { get => _column; }

    private TerrainCube[,] _board;
    private HashSet<(int, int)> _visitedGrids = new HashSet<(int, int)>();

    void Awake()
    {
        _environmentalChangeHandler.Initialize(this, _terrainTransitionRules);
        _board = new TerrainCube[_row, _column];
        for(int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                _board[i, j] = Instantiate(_terrainCubePrefab, new Vector3(i * _spacing, 0, j * _spacing), Quaternion.identity);
                _board[i, j].TerrainAnimationControllerInitialize(_terrainManager);
                _board[i, j].TerrainType = (TerrainType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(TerrainType)).Length);
            }
        }
    }

    /// <summary>
    /// 地形を変更する
    /// </summary>
    /// <param name="type">新しい地形</param>
    /// <param name="gridPos">座標</param>
    public void ChangeTerrainType(TerrainType type, (int,int) gridPos)
    {
        _board[gridPos.Item1, gridPos.Item2].TerrainType = type;
    }

    /// <summary>
    /// 環境変化の前に全キューブの現在の地形を「古い地形」として保存
    /// </summary>
    public void SaveCurrentTerrainAsOld()
    {
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Column; c++)
            {
                if (_board[r, c] != null)
                {
                    _board[r, c].SaveCurrentAsOld();
                }
            }
        }
    }

    /// <summary>
    /// 周囲の地形を変化させる。
    /// <para>地形が変化した場合、そこからさらに変化させる</para>
    /// </summary>
    /// <param name="gridPos">中央の座標</param>
    /// <param name="type">中央の地形</param>
    public void TopographicalChanges((int, int) gridPos, TerrainType type)
    {
        if (_visitedGrids.Contains(gridPos)) { return; }
        _visitedGrids.Add(gridPos);

        try
        {
            (int, int)[] aroundGrid = new (int, int)[5]
            {
            gridPos,
            (gridPos.Item1, gridPos.Item2 - 1),
            (gridPos.Item1, gridPos.Item2 + 1),
            (gridPos.Item1 - 1, gridPos.Item2),
            (gridPos.Item1 + 1, gridPos.Item2),
            };

            for (int i = 0; i < aroundGrid.Length; i++)
            {
                if (aroundGrid[i].Item1 < 0 || aroundGrid[i].Item1 >= _board.GetLength(0)) { continue; }
                if (aroundGrid[i].Item2 < 0 || aroundGrid[i].Item2 >= _board.GetLength(1)) { continue; }

                if (i != 0 && _visitedGrids.Contains(aroundGrid[i])) { continue; }

                if (_terrainTransitionRules.TryTransitionTerrain(type, _board[aroundGrid[i].Item1, aroundGrid[i].Item2].TerrainType, out TerrainType newType))
                {
                    ChangeTerrainType(newType, aroundGrid[i]);

                    if (i != 0)
                    {
                        TopographicalChanges(aroundGrid[i], newType);
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        ChangeTerrainType(type, aroundGrid[i]);
                    }
                }
            }
        }
        finally
        {
            _visitedGrids.Remove(gridPos);
        }
    }

    public TerrainType GetTerrainType((int, int) gridPos)
    {
        return _board[gridPos.Item1, gridPos.Item2].TerrainType;
    }

    /// <summary>
    /// タイルを置いた時の地形の変化
    /// </summary>
    /// <param name="type1">地形1</param>
    /// <param name="type2">地形2</param>
    /// <param name="newType">変化後の地形</param>
    /// <returns>変化したか</returns>
    public bool TopographicalChanges(TerrainType type1, TerrainType type2, out TerrainType newType)
    {
        return _terrainTransitionRules.TryTransitionTerrain(type1, type2, out newType);
    }

    /// <summary>
    /// 環境を変化させる。砂漠化と火災の拡散を行う。
    /// </summary>
    /// <param name="isSpreadFire">火災の拡散を行うか</param>
    public void EnvironmentalChanges(bool isSpreadFire)
    {
        _environmentalChangeHandler.ApplyEnvironmentalChanges(isSpreadFire);
    }

    /// <summary>
    /// 全キューブのアニメーションを実行し、完了まで待機するコルーチン
    /// </summary>
    public IEnumerator ChangeNewStyleRoutine(float fadeDuration)
    {
        var activeCoroutines = new List<Coroutine>();

        // すべてのキューブのアニメーションコルーチンを開始
        for (int r = 0; r < Row; r++)
        {
            for (int c = 0; c < Column; c++)
            {
                if (_board[r, c] != null)
                {
                    var coroutine = StartCoroutine(_board[r, c].ChangeTerrainStyle(fadeDuration));
                    activeCoroutines.Add(coroutine);
                }
            }
        }

        // すべてのコルーチンが完了するまで待機
        foreach (var coroutine in activeCoroutines)
        {
            yield return coroutine;
        }
    }

    public void ChangeNewStyle(float fadeDuration, UnityEngine.Events.UnityAction last)
    {
        StartCoroutine(ChangeNewStyleRoutine(fadeDuration));
        last?.Invoke();
    }

    /// <summary>
    /// world座標をグリッド上の座標に変換する。
    /// </summary>
    /// <param name="worldPos">world座標</param>
    /// <param name="gridPos">グリッド上の座標</param>
    /// <returns>変換できたか</returns>
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
