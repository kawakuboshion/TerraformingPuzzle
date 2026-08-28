using UnityEngine;

/// <summary>
/// カード生成ロジックを管理するクラス
/// HandSlots から分離されたカード生成と確率計算
/// </summary>
public class CardGenerator : MonoBehaviour
{
    private CardConfig _cardConfig;
    private int _totalProbability;

    public void Initialize(CardConfig cardConfig)
    {
        _cardConfig = cardConfig;
        _totalProbability = _cardConfig.GetTotalProbability();
    }

    /// <summary>
    /// 確率に基づいてランダムな地形タイプを取得
    /// </summary>
    public TerrainType GetRandomTerrainType()
    {
        if (_cardConfig == null)
        {
            Debug.LogError("CardGenerator not initialized. Call Initialize() first.");
            return TerrainType.Water;
        }

        int randomValue = Random.Range(0, _totalProbability);
        int cumulativeProbability = 0;

        TerrainType[] terrainTypes = _cardConfig.TerrainTypes;
        int[] probabilities = _cardConfig.TerrainTypeProbabilities;

        for (int i = 0; i < terrainTypes.Length; i++)
        {
            if (i < probabilities.Length)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue < cumulativeProbability)
                {
                    return terrainTypes[i];
                }
            }
        }

        // デフォルト値
        return TerrainType.Water;
    }
}
