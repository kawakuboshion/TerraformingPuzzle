using UnityEngine;

/// <summary>
/// ターン進行ロジックを管理するクラス
/// GameManager から分離されたターン関連の状態管理
/// </summary>
public class TurnManager : MonoBehaviour
{
    private int _turnCount = 0;
    private bool _playerChangeApplied = false;
    private bool _environmentalChangeApplied = false;

    public int TurnCount => _turnCount;
    public bool PlayerChangeApplied => _playerChangeApplied;
    public bool EnvironmentalChangeApplied => _environmentalChangeApplied;

    public void ResetTurn()
    {
        _playerChangeApplied = false;
        _environmentalChangeApplied = false;
    }

    public void MarkPlayerChangeApplied()
    {
        _playerChangeApplied = true;
    }

    public void MarkEnvironmentalChangeApplied()
    {
        _environmentalChangeApplied = true;
    }

    public void AdvanceTurn()
    {
        _turnCount++;
    }

    public void ResetTurnCount()
    {
        _turnCount = 0;
    }
}
