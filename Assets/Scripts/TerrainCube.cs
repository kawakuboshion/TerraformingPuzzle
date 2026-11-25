using System.Collections;
using UnityEngine;

public class TerrainCube : MonoBehaviour
{
    [SerializeField] private TerrainType _terrainType;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private GameObject _plants;
    [SerializeField] private TerrainAnimationController _animationController;
    [SerializeField] private TerrainConfig _terrainConfig;

    public TerrainManager TerrainManager { private get; set; }

    public TerrainType TerrainType
    {
        get { return _terrainType; }
        set { _terrainType = value; }
    }

    public void PreviewTerrain(TerrainType previewType)
    {
        _animationController.PreviewTerrain(
            _renderer,
            _plants,
            previewType,
            transform);
    }

    public void ClearPreview()
    {
        _animationController.PreviewTerrain(
            _renderer,
            _plants,
            _terrainType,
            transform);
    }

    private TerrainType _oldTerrainType;
    private bool _isChangeNewStyle = false;

    public IEnumerator ChangeTerrainStyle(float fadeDuration, UnityEngine.Events.UnityAction<IEnumerator> callback)
    {
        if (_isChangeNewStyle)
        {
            _oldTerrainType = _terrainType;
            _isChangeNewStyle = false;
        }

        yield return _animationController.PlayTerrainTransitionAnimation(
            _renderer,
            _plants,
            _oldTerrainType,
            _terrainType,
            transform,
            fadeDuration,
            callback);

        _isChangeNewStyle = true;
    }

    public void TerrainAnimationControllerInitialize(TerrainManager terrainManager)
    {
        _animationController.Initialize(terrainManager, _terrainConfig);
    }
}