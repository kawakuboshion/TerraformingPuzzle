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

    private int _score = 0;
    private bool _isGameEnd = false;
    private bool _isAnimationRunning = false;

    void Start()
    {
        _scoreSlider.maxValue = _gameConfig.TargetScore;
        _scoreSlider.minValue = 0;
        SetScoreUI();
        UpdateTurnCountDisplay();
        _resultCanvas.gameObject.SetActive(false);
        
        // 初期化を同期的に行う（Start 終了後に最初のフレームで初期アニメーションを開始）
    }

    private enum TurnPhase
    {
        InitialAnimation,            // ゲーム開始時のアニメーション
        WaitingForPlayerAction,
        ShowingPlayerChangeAnimation,
        AdvancingTurn,
        ShowingEnvironmentalChangeAnimation
    }

    private TurnPhase _currentPhase = TurnPhase.InitialAnimation;

    void Update()
    {
        if (_isGameEnd) return;

        switch (_currentPhase)
        {
            case TurnPhase.InitialAnimation:
                // ゲーム開始時の初期アニメーション
                StartCoroutine(PlayInitialAnimation());
                _currentPhase = TurnPhase.WaitingForPlayerAction;
                break;

            case TurnPhase.WaitingForPlayerAction:
                if (_action.CanMove || _isAnimationRunning) return;
                StartCoroutine(PlayPlayerChangeAnimation());
                _currentPhase = TurnPhase.ShowingPlayerChangeAnimation;
                break;

            case TurnPhase.ShowingPlayerChangeAnimation:
                if (_isAnimationRunning) return;  // アニメーション実行中
                _turnManager.ResetTurn();
                _turnManager.AdvanceTurn();
                _currentPhase = TurnPhase.AdvancingTurn;
                break;

            case TurnPhase.AdvancingTurn:
                StartCoroutine(PlayEnvironmentalChangeAnimation());
                _currentPhase = TurnPhase.ShowingEnvironmentalChangeAnimation;
                break;

            case TurnPhase.ShowingEnvironmentalChangeAnimation:
                if (_isAnimationRunning) return;  // アニメーション実行中
                CheckGameState();
                _currentPhase = TurnPhase.WaitingForPlayerAction;
                _action.CanMove = true;
                break;
        }
    }

    /// <summary>
    /// ゲーム開始時の初期アニメーション
    /// </summary>
    private IEnumerator PlayInitialAnimation()
    {
        _isAnimationRunning = true;
        yield return _gridManager.ChangeNewStyleRoutine(_gameConfig.AnimationDuration);
        _isAnimationRunning = false;
    }

    /// <summary>
    /// プレイヤー行動後の地形変化アニメーションを再生
    /// </summary>
    private IEnumerator PlayPlayerChangeAnimation()
    {
        _isAnimationRunning = true;
        yield return _gridManager.ChangeNewStyleRoutine(_gameConfig.AnimationDuration);
        _isAnimationRunning = false;
    }

    /// <summary>
    /// 環境変化処理とアニメーションを実行
    /// </summary>
    private IEnumerator PlayEnvironmentalChangeAnimation()
    {
        _isAnimationRunning = true;
        
        // 環境変化の前に、全キューブの現在の地形を保存する
        // これにより、複数のテラインが同時に地形変更される場合でも、
        // 各テラインが正しい「古い地形」と「新しい地形」を保持できる
        _gridManager.SaveCurrentTerrainAsOld();
        
        // 環境変化を実行（複数のテラインで地形が変更される可能性あり）
        _gridManager.EnvironmentalChanges(_turnManager.TurnCount % _gameConfig.FireSpreadRate == 0);
        
        // すべてのテラインでアニメーションを実行
        yield return _gridManager.ChangeNewStyleRoutine(_gameConfig.AnimationDuration);
        
        _isAnimationRunning = false;
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
