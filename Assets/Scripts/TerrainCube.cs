using UnityEngine;

public class TerrainCube : MonoBehaviour
{
    [SerializeField] private TerrainType _terrainType;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _plants; 
    public TerrainManager _terrainManager { private get; set; }
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
        if(_renderer != null && _terrainManager != null)
        {
            _renderer.material = _terrainManager.GetTerrainMaterial(terrainType);
        }
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, _heightScales[(int)_terrainType], scale.z);

        if(terrainType == TerrainType.FertileLand)
        {
            _plants.SetActive(true);
        }
        else
        {
            _plants.SetActive(false);
        }
    }
}