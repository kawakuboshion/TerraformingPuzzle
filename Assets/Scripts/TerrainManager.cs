using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField]
    private Color[] _terrainColors = new Color[]
    {
        Color.gray, Color.gray, Color.blue, Color.brown, Color.darkRed, Color.green, Color.yellowNice, Color.orange 
    };
    private string[] _terrainNames = new string[]
    {
        "空き地","荒野","水","土","泥","肥沃な大地","砂漠","火"
    };

    public string GetTerrainName(TerrainType terrainType)
    {
        return _terrainNames[(int)terrainType];
    }

    public Color GetTerrainColor(TerrainType terrainType)
    {
        return _terrainColors[((int)terrainType)];
    }
}

public enum TerrainType
{
    VacantLot,
    Wasteland,
    Water,
    Soil,
    Mud,
    FertileLand,
    Desert,
    Fire,
}
