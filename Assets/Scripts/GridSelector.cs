using UnityEngine;
using UnityEngine.InputSystem;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private GameObject _selectCursol;
    private Vector3 _selectPos;
    private bool _selected;

    private void Start()
    {
        _selectCursol.SetActive(false);
    }

    private void Update()
    {
        UpdateMousePosition();
    }

    private void UpdateMousePosition()
    {
        Pointer pointer = Pointer.current;
        if (pointer == null) { return; }

        // クリック位置を取得
        Vector3 position = pointer.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out RaycastHit  hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.TryGetComponent(out TerrainCube hitTerrain))
            {
                _selectCursol.transform.position = hitTerrain.transform.position + new Vector3(0, 0.15f, 0);
                _selectCursol.transform.localScale = hit.collider.gameObject.transform.localScale + new Vector3(0, 0.5f, 0);
                _selectCursol.SetActive(true);

                _selectPos = hitTerrain.transform.position;
                _selected = true;
            }
            else
            {
                _selectCursol.SetActive(false);
                _selectPos = Vector3.zero;
                _selected = false;
            }
        }
        else
        {
            _selectCursol.SetActive(false);
            _selectPos = Vector3.zero;
            _selected = false;
        }
    }

    public bool TryGetSelectPos(out Vector3 pos)
    {
        if (_selected)
        {
            pos = _selectPos;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}
