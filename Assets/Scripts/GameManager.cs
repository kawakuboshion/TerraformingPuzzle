using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private PlayerAction _action;
    [SerializeField] private HandSlots _handSlots;
    [SerializeField] private TextMeshProUGUI _turnCountText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Canvas _resultCanvas;
    [SerializeField] private Slider _scoreSlider;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private TurnManager _turnManager;

    private IEnumerator _changeStyle = null;
    private int _score = 0;
    private bool _isGameEnd = false;

    void Start()
    {
        _scoreSlider.maxValue = _gameConfig.TargetScore;
        _scoreSlider.minValue = 0;
        SetScoreUI();
        UpdateTurnCountDisplay();
        _resultCanvas.gameObject.SetActive(false);
        _gridManager.ChangeNewStyle(_gameConfig.AnimationDuration, change => _changeStyle = change);
    }

    void Update()
    {
        // ターンの進行
        if (!_action.CanMove && !_isGameEnd && _changeStyle == null)
        {
            // プレイヤーの行動が終わったら環境変化を適用する
            if (!_turnManager.PlayerChangeApplied)
            {
                _gridManager.ChangeNewStyle(_gameConfig.AnimationDuration, change => _changeStyle = change);
                _turnManager.MarkPlayerChangeApplied();
                return;
            }

            // 環境変化が適用されたらターンを進める
            if (!_turnManager.EnvironmentalChangeApplied)
            {
                _turnManager.ResetTurn();
                _action.CanMove = true;
                _turnManager.AdvanceTurn();
                _turnManager.MarkEnvironmentalChangeApplied();
                return;
            }
            else
            {
                _gridManager.EnvironmentalChanges(_turnManager.TurnCount % _gameConfig.FireSpreadRate == 0);
                _gridManager.ChangeNewStyle(_gameConfig.AnimationDuration, change => _changeStyle = change);
                _turnManager.ResetTurn();
            }

            CheckGameState();
        }
    }

    private void CheckGameState()
    {
        if (_score >= _gameConfig.TargetScore)
        {
            EndGame("GameClear");
        }
        else if (_turnManager.TurnCount > _gameConfig.TurnLimit || _handSlots.GetCardCount() <= 0)
        {
            EndGame("GameOver");
        }
        else
        {
            UpdateTurnCountDisplay();
        }
    }

    private void EndGame(string resultMessage)
    {
        _resultCanvas.gameObject.SetActive(true);
        _action.CanMove = false;
        _isGameEnd = true;
        _resultText.text = resultMessage;
    }

    private void UpdateTurnCountDisplay()
    {
        _turnCountText.text = $"{_turnManager.TurnCount} / {_gameConfig.TurnLimit}";
    }

    public void SetScoreUI()
    {
        _scoreText.text = $"{_score} / {_gameConfig.TargetScore}";
        _scoreSlider.value = _score;
    }

    public void AddScore(int point)
    {
        _score += point;
        SetScoreUI();
    }

    public void MinusScore(int point)
    {
        _score -= point;
        SetScoreUI();
    }
}
