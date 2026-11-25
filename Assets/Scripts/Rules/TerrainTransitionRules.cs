using UnityEngine;

/// <summary>
/// 地形変化ルールを管理するクラス
/// GridManager から分離された地形遷移ロジック
/// </summary>
public class TerrainTransitionRules : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    /// <summary>
    /// カード配置前のプレビュー用。スコア加算や環境確率を発生させずに変化先を返す。
    /// </summary>
    public bool TryPreviewTransition(TerrainType type1, TerrainType type2, out TerrainType newType)
    {
        switch (type1)
        {
            case TerrainType.Water:
                switch (type2)
                {
                    case TerrainType.Soil:
                        newType = TerrainType.Mud;
                        return true;
                    case TerrainType.Mud:
                        newType = TerrainType.FertileLand;
                        return true;
                    case TerrainType.Fire:
                        newType = TerrainType.VacantLot;
                        return true;
                    case TerrainType.Desert:
                        newType = TerrainType.Soil;
                        return true;
                }
                break;

            case TerrainType.Soil:
                if (type2 == TerrainType.Water)
                {
                    newType = TerrainType.Mud;
                    return true;
                }
                break;

            case TerrainType.Mud:
                if (type2 == TerrainType.Water)
                {
                    newType = TerrainType.FertileLand;
                    return true;
                }
                break;

            case TerrainType.Fire:
                switch (type2)
                {
                    case TerrainType.Water:
                        newType = TerrainType.VacantLot;
                        return true;
                    case TerrainType.Soil:
                    case TerrainType.FertileLand:
                        newType = TerrainType.Fire;
                        return true;
                }
                break;

            case TerrainType.Desert:
                if (type2 == TerrainType.Water)
                {
                    newType = TerrainType.Soil;
                    return true;
                }
                break;
        }

        newType = type2;
        return false;
    }

    /// <summary>
    /// 2つの地形が接触した時の遷移ルール
    /// </summary>
    public bool TryTransitionTerrain(TerrainType type1, TerrainType type2, out TerrainType newType)
    {
        if (type1 == TerrainType.VacantLot || type1 == TerrainType.Wasteland)
        {
            newType = type1;
            return false;
        }

        if (type1 == TerrainType.Desert)
        {
            if ((type2 == TerrainType.FertileLand || type2 == TerrainType.Soil) &&
                Random.Range(0, 100) <= TerrainConfig.DESERTIFICATION_PROBABILITY)
            {
                newType = TerrainType.Desert;
                if (type2 == TerrainType.FertileLand)
                {
                    _gameManager.MinusScore(GameConfig.SCORE_FERTILE_LAND_DESTROYED);
                }
                return true;
            }
        }

        if (!TryPreviewTransition(type1, type2, out newType))
        {
            return false;
        }

        if ((type1 == TerrainType.Water && type2 == TerrainType.Mud) ||
            (type1 == TerrainType.Mud && type2 == TerrainType.Water))
        {
            _gameManager.AddScore(GameConfig.SCORE_WATER_MUD_TRANSITION);
        }

        if ((type1 == TerrainType.Water && type2 == TerrainType.Fire) ||
            (type1 == TerrainType.Fire && type2 == TerrainType.Water))
        {
            _gameManager.AddScore(GameConfig.SCORE_EXTINGUISH_FIRE);
        }

        if (type1 == TerrainType.Fire && type2 == TerrainType.FertileLand)
        {
            _gameManager.MinusScore(GameConfig.SCORE_FERTILE_LAND_DESTROYED);
        }

        return true;
    }
}
