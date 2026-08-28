using UnityEngine;

/// <summary>
/// ゲーム全体の定数・設定を管理するクラス
/// GameManager の magic numbers をここに集約
/// </summary>
public class GameConfig : MonoBehaviour
{
    [SerializeField] private int _fireSpreadRate = 3;
    [SerializeField] private int _turnLimit = 20;
    [SerializeField] private int _targetScore = 3000;
    [SerializeField] private float _animationDuration = 2f;

    public int FireSpreadRate => _fireSpreadRate;
    public int TurnLimit => _turnLimit;
    public int TargetScore => _targetScore;
    public float AnimationDuration => _animationDuration;

    // Named constants for commonly used score values
    public const int SCORE_WATER_MUD_TRANSITION = 100;
    public const int SCORE_EXTINGUISH_FIRE = 50;
    public const int SCORE_FERTILE_LAND_DESTROYED = 100;
}
