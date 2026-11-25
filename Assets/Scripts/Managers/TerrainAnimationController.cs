using System.Collections;
using UnityEngine;

/// <summary>
/// 地形キューブのアニメーション処理を管理するクラス
/// TerrainCube から分離されたアニメーション関連ロジック
/// </summary>
public class TerrainAnimationController : MonoBehaviour
{
    private TerrainManager _terrainManager;
    private TerrainConfig _terrainConfig;
    private MaterialPropertyBlock _previewProperties;

    public void Initialize(TerrainManager terrainManager, TerrainConfig terrainConfig)
    {
        _terrainManager = terrainManager;
        _terrainConfig = terrainConfig;
    }

    /// <summary>
    /// 地形の遷移アニメーションを再生
    /// </summary>
    public IEnumerator PlayTerrainTransitionAnimation(
        MeshRenderer renderer,
        GameObject plantsObject,
        TerrainType oldTerrainType,
        TerrainType newTerrainType,
        Transform cubeTransform,
        float fadeDuration)
    {
        float elapsedTime = 0f;
        renderer.SetPropertyBlock(null);
        renderer.material.SetColor("_ColorA", _terrainManager.GetTerrainColor(oldTerrainType));
        renderer.material.SetColor("_ColorB", _terrainManager.GetTerrainColor(newTerrainType));
        renderer.material.SetFloat("_Transition", 0f);

        Vector3 oldScale = cubeTransform.localScale;
        float targetHeightScale = _terrainConfig.GetHeightScale(newTerrainType);

        // 肥沃な大地の場合は植物を表示、それ以外の場合は非表示にする
        if (newTerrainType == TerrainType.FertileLand)
        {
            plantsObject.SetActive(true);
        }
        else
        {
            plantsObject.SetActive(false);
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeDuration);

            yield return new WaitForEndOfFrame();

            if (renderer != null && _terrainManager != null)
            {
                renderer.material.SetFloat("_Transition", normalizedTime);
                cubeTransform.localScale = Vector3.Lerp(
                    oldScale,
                    new Vector3(oldScale.x, targetHeightScale, oldScale.z),
                    normalizedTime);
            }
        }
    }

    /// <summary>
    /// カード配置前に変化結果だけを見た目へ反映する。
    /// 実際の地形データは変更しない。
    /// </summary>
    public void PreviewTerrain(
        MeshRenderer renderer,
        GameObject plantsObject,
        TerrainType previewType,
        Transform cubeTransform)
    {
        if (_previewProperties == null)
        {
            _previewProperties = new MaterialPropertyBlock();
        }

        renderer.GetPropertyBlock(_previewProperties);
        _previewProperties.SetColor("_ColorA", _terrainManager.GetTerrainColor(previewType));
        _previewProperties.SetColor("_ColorB", _terrainManager.GetTerrainColor(previewType));
        _previewProperties.SetFloat("_Transition", 1f);
        renderer.SetPropertyBlock(_previewProperties);
        cubeTransform.localScale = new Vector3(
            cubeTransform.localScale.x,
            _terrainConfig.GetHeightScale(previewType),
            cubeTransform.localScale.z);

        plantsObject.SetActive(previewType == TerrainType.FertileLand);
    }

    public void ClearPreview(MeshRenderer renderer, GameObject plantsObject, TerrainType terrainType)
    {
        renderer.SetPropertyBlock(null);
        plantsObject.SetActive(terrainType == TerrainType.FertileLand);
    }
}
