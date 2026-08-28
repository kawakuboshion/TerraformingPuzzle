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
        float fadeDuration,
        UnityEngine.Events.UnityAction<IEnumerator> callback)
    {
        float elapsedTime = 0f;
        renderer.material.SetColor("_ColorA", _terrainManager.GetTerrainColor(oldTerrainType));
        renderer.material.SetColor("_ColorB", _terrainManager.GetTerrainColor(newTerrainType));

        // 肥沃な大地以外は植物を非表示
        if (newTerrainType != TerrainType.FertileLand)
        {
            plantsObject.SetActive(false);
        }

        Vector3 oldScale = cubeTransform.localScale;
        float targetHeightScale = _terrainConfig.GetHeightScale(newTerrainType);

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

        // 肥沃な大地の場合は植物を表示
        if (newTerrainType == TerrainType.FertileLand)
        {
            plantsObject.SetActive(true);
        }

        callback?.Invoke(null);
    }
}
