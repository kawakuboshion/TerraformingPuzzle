using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GridSelector _gridSelector;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private HandSlots _handSlots;

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) { return; }
        if (mouse.leftButton.wasPressedThisFrame)
        {
            if (_gridSelector.TryGetSelectPos(out Vector3 pos) && _gridManager.TryWorldPositionToGridPosition(pos, out (int, int) gridPos))
            {
                if(_handSlots.TryGetUseCard(out TerrainType type))
                {
                    Debug.Log(type);
                    _gridManager.ChangeTerrainType(type, gridPos);
                }
            }
        }
    }
}
