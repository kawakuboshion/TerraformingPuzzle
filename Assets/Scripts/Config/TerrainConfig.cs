using UnityEngine;

/// <summary>
/// 地形に関する設定を管理するクラス
/// TerrainCube から分離された地形の高さスケールと定数
/// </summary>
public class TerrainConfig : MonoBehaviour
{
    [SerializeField] private float[] _heightScales = new float[8]
    {
        0.2f,  // VacantLot
        0.2f,  // Wasteland
        0.1f,  // Water
        0.4f,  // Soil
        0.3f,  // Mud
        0.5f,  // FertileLand
        0.2f,  // Desert
        0.6f   // Fire
    };

    public float[] HeightScales => _heightScales;

    public float GetHeightScale(TerrainType terrainType)
    {
        return _heightScales[(int)terrainType];
    }

    // Named constants for desertification logic
    public const int DESERTIFICATION_PROBABILITY = 20; // 20% chance
}
