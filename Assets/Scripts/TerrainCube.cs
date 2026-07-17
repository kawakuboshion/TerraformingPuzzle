using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TerrainCube : MonoBehaviour
{
    [SerializeField] private TerrainType _terrainType;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material[] _materials;
    public TerrainType TerrainType {  get { return _terrainType; } set { ChangeTerrainType(value); } }
    private float[] _heightScales = new float[8]
    {
        0.2f, 0.2f, 0.1f, 0.4f, 0.6f, 0.3f, 0.5f, 0.2f
    };

    private void OnValidate()
    {
        ChangeTerrainType(TerrainType);
    }

    private void ChangeTerrainType(TerrainType terrainType)
    {
        _terrainType = terrainType;
        if(_renderer != null)
        {
            _renderer.material = _materials[(int)_terrainType];
        }
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, _heightScales[(int)_terrainType], scale.z);
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