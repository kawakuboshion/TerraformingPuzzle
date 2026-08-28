using UnityEngine;

/// <summary>
/// 環境変化（砂漠化と火災拡散）を管理するクラス
/// GridManager から分離された環境変化ロジック
/// </summary>
public class EnvironmentalChangeHandler : MonoBehaviour
{
    private GridManager _gridManager;
    private TerrainTransitionRules _terrainTransitionRules;

    public void Initialize(GridManager gridManager, TerrainTransitionRules terrainTransitionRules)
    {
        _gridManager = gridManager;
        _terrainTransitionRules = terrainTransitionRules;
    }

    /// <summary>
    /// 砂漠化と火災の拡散を行う環境変化
    /// </summary>
    public void ApplyEnvironmentalChanges(bool isSpreadFire)
    {
        int rows = _gridManager.Row;
        int columns = _gridManager.Column;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (isSpreadFire)
                {
                    SpreadTerrainType(r, c, TerrainType.Fire);
                }
                SpreadTerrainType(r, c, TerrainType.Desert);
            }
        }
    }

    /// <summary>
    /// 指定座標を中心に地形を拡散させる
    /// </summary>
    private void SpreadTerrainType(int row, int col, TerrainType terrainType)
    {
        TerrainType currentType = _gridManager.GetTerrainType((row, col));

        if (currentType != terrainType)
        {
            return;
        }

        // 周囲4方向の座標
        (int, int)[] adjacentPositions = new (int, int)[4]
        {
            (row, col - 1),
            (row, col + 1),
            (row - 1, col),
            (row + 1, col)
        };

        for (int i = 0; i < adjacentPositions.Length; i++)
        {
            int adjacentRow = adjacentPositions[i].Item1;
            int adjacentCol = adjacentPositions[i].Item2;

            // グリッド範囲外チェック
            if (adjacentRow < 0 || adjacentRow >= _gridManager.Row) continue;
            if (adjacentCol < 0 || adjacentCol >= _gridManager.Column) continue;

            TerrainType adjacentType = _gridManager.GetTerrainType((adjacentRow, adjacentCol));

            if (_terrainTransitionRules.TryTransitionTerrain(terrainType, adjacentType, out TerrainType newType))
            {
                _gridManager.ChangeTerrainType(newType, (adjacentRow, adjacentCol));
            }
        }
    }
}
