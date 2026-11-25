using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GridSelector _gridSelector;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private HandSlots _handSlots;

    private bool _canMove = true;

    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }

    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) { return; }
        if (!_canMove)
        {
            _gridManager.ClearPreview();
            return;
        }

        if (_gridSelector.TryGetSelectPos(out Vector3 hoverPos) &&
            _gridManager.TryWorldPositionToGridPosition(hoverPos, out (int, int) hoverGridPos) &&
            _handSlots.TryGetUseCard(out TerrainType hoverType))
        {
            _gridManager.PreviewTopographicalChanges(hoverGridPos, hoverType);
        }
        else
        {
            _gridManager.ClearPreview();
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (_gridSelector.TryGetSelectPos(out Vector3 pos) && _gridManager.TryWorldPositionToGridPosition(pos, out (int, int) gridPos))
            {
                if (_handSlots.TryGetUseCard(out TerrainType type))
                {
                    if (_gridManager.GetTerrainType(gridPos) != type && _gridManager.GetTerrainType(gridPos) != TerrainType.FertileLand)
                    {
                        _gridManager.TopographicalChanges(gridPos, type);
                        _handSlots.UseCard();
                        _canMove = false;
                    }
                }
            }
        }
    }
}
