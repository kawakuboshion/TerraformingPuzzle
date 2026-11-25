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
        set
        {
            if (_terrainType != value && !_hasSavedOldTerrain)
            {
                _oldTerrainType = _terrainType;
                _hasSavedOldTerrain = true;
            }
            _terrainType = value;
        }
    }

    public void PreviewTerrain(TerrainType previewType)
    {
        if (!_isPreviewing)
        {
            _previewScale = transform.localScale;
            _isPreviewing = true;
        }

        _animationController.PreviewTerrain(
            _renderer,
            _plants,
            previewType,
            transform);
    }

    public void ClearPreview()
    {
        if (!_isPreviewing)
        {
            return;
        }

        _isPreviewing = false;
        _animationController.ClearPreview(_renderer, _plants, _terrainType);
        transform.localScale = _previewScale;
    }

    private TerrainType _oldTerrainType;
    private Vector3 _previewScale;
    private bool _isPreviewing;
    private bool _hasSavedOldTerrain;

    private void Start()
    {
        // 初回時の_oldTerrainTypeを現在の値で初期化
        _oldTerrainType = _terrainType;
    }

    public IEnumerator ChangeTerrainStyle(float fadeDuration)
    {
        ClearPreview();

        // _oldTerrainType はプロパティの setter で既に設定されている
        // TerrainType プロパティが set される時点で、古い値を記録している

        yield return _animationController.PlayTerrainTransitionAnimation(
            _renderer,
            _plants,
            _oldTerrainType,
            _terrainType,
            transform,
            fadeDuration);
        _oldTerrainType = _terrainType;
        _hasSavedOldTerrain = false;
    }

    public void TerrainAnimationControllerInitialize(TerrainManager terrainManager)
    {
        _animationController.Initialize(terrainManager, _terrainConfig);
    }

    /// <summary>
    /// 現在の地形を「古い地形」として保存
    /// 環境変化前に呼び出され、複数の地形変更が起きても _oldTerrainType が上書きされないようにする
    /// </summary>
    public void SaveCurrentAsOld()
    {
        _oldTerrainType = _terrainType;
        _hasSavedOldTerrain = true;
    }
}