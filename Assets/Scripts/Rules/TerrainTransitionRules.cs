using UnityEngine;

/// <summary>
/// 地形変化ルールを管理するクラス
/// GridManager から分離された地形遷移ロジック
/// </summary>
public class TerrainTransitionRules : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    /// <summary>
    /// 2つの地形が接触した時の遷移ルール
    /// </summary>
    public bool TryTransitionTerrain(TerrainType type1, TerrainType type2, out TerrainType newType)
    {
        switch (type1)
        {
            case TerrainType.Water:
                return TryWaterTransition(type2, out newType);

            case TerrainType.Soil:
                return TrySoilTransition(type2, out newType);

            case TerrainType.Fire:
                return TryFireTransition(type2, out newType);

            case TerrainType.Mud:
                return TryMudTransition(type2, out newType);

            case TerrainType.Desert:
                return TryDesertTransition(type2, out newType);
        }

        newType = type1;
        return false;
    }

    private bool TryWaterTransition(TerrainType type2, out TerrainType newType)
    {
        switch (type2)
        {
            case TerrainType.Soil:
                newType = TerrainType.Mud;
                return true;

            case TerrainType.Mud:
                newType = TerrainType.FertileLand;
                _gameManager.AddScore(GameConfig.SCORE_WATER_MUD_TRANSITION);
                return true;

            case TerrainType.Fire:
                newType = TerrainType.VacantLot;
                _gameManager.AddScore(GameConfig.SCORE_EXTINGUISH_FIRE);
                return true;

            case TerrainType.Desert:
                newType = TerrainType.Soil;
                return true;
        }

        newType = type2;
        return false;
    }

    private bool TrySoilTransition(TerrainType type2, out TerrainType newType)
    {
        if (type2 == TerrainType.Water)
        {
            newType = TerrainType.Mud;
            return true;
        }

        newType = type2;
        return false;
    }

    private bool TryFireTransition(TerrainType type2, out TerrainType newType)
    {
        switch (type2)
        {
            case TerrainType.Water:
                newType = TerrainType.VacantLot;
                _gameManager.AddScore(GameConfig.SCORE_EXTINGUISH_FIRE);
                return true;

            case TerrainType.FertileLand:
                newType = TerrainType.Fire;
                _gameManager.MinusScore(GameConfig.SCORE_FERTILE_LAND_DESTROYED);
                return true;

            case TerrainType.Soil:
                newType = TerrainType.Fire;
                return true;
        }

        newType = type2;
        return false;
    }

    private bool TryMudTransition(TerrainType type2, out TerrainType newType)
    {
        if (type2 == TerrainType.Water)
        {
            newType = TerrainType.FertileLand;
            _gameManager.AddScore(GameConfig.SCORE_WATER_MUD_TRANSITION);
            return true;
        }

        newType = type2;
        return false;
    }

    private bool TryDesertTransition(TerrainType type2, out TerrainType newType)
    {
        switch (type2)
        {
            case TerrainType.FertileLand:
            case TerrainType.Soil:
                if (Random.Range(0, 100) <= TerrainConfig.DESERTIFICATION_PROBABILITY)
                {
                    newType = TerrainType.Desert;
                    if (type2 == TerrainType.FertileLand)
                    {
                        _gameManager.MinusScore(GameConfig.SCORE_FERTILE_LAND_DESTROYED);
                    }
                    return true;
                }
                break;

            case TerrainType.Water:
                newType = TerrainType.Soil;
                return true;
        }

        newType = type2;
        return false;
    }
}
