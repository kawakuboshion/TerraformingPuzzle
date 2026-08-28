using UnityEngine;

/// <summary>
/// カード生成に関する設定を管理するクラス
/// HandSlots から分離されたカード確率配置
/// </summary>
public class CardConfig : MonoBehaviour
{
    [SerializeField] private int _slotCount = 3;
    [SerializeField] private int[] _terrainTypeProbabilities = new int[]
    {
        50, // Water: 50%
        30, // Soil: 30%
        20  // Mud: 20%
    };
    [SerializeField] private TerrainType[] _terrainTypes = new TerrainType[]
    {
        TerrainType.Water,
        TerrainType.Soil,
        TerrainType.Mud
    };

    public int SlotCount => _slotCount;
    public int[] TerrainTypeProbabilities => _terrainTypeProbabilities;
    public TerrainType[] TerrainTypes => _terrainTypes;

    public int GetTotalProbability()
    {
        int total = 0;
        for (int i = 0; i < _terrainTypeProbabilities.Length; i++)
        {
            total += _terrainTypeProbabilities[i];
        }
        return total;
    }
}
