using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] private Material[] _materials;
    private string[] _terrainNames = new string[]
    {
        "空き地","荒野","水","土","火","泥","肥沃な大地","砂漠"
    };

    public string GetTerrainName(TerrainType terrainType)
    {
        return _terrainNames[(int)terrainType];
    }

    public Material GetTerrainMaterial(TerrainType terrainType)
    {
        return _materials[(int)terrainType];
    }

    public Color GetTerrainColor(TerrainType terrainType)
    {
        return _materials[((int)terrainType)].color;
    }
}

public enum TerrainType
{
    VacantLot,
    Wasteland,
    Water,
    Soil,
    Fire,
    Mud,
    FertileLand,
    Desert,
}
