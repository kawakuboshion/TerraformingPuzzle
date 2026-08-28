using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _selectedObject;
    [SerializeField] private TerrainType _terrainType = TerrainType.VacantLot;
    [SerializeField] private string _name;

    private bool _isSelected = false;
    private HandSlots _handSlots;
    private TerrainManager _terrainManager;
    private int _handSlotIndex;

    public TerrainType TerrainType
    {
        get { return _terrainType; }
        set { ChangeTerrainType(value); }
    }

    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }

    public HandSlots HandSlots
    {
        get { return _handSlots; }
        set { _handSlots = value; }
    }

    public TerrainManager TerrainManager
    {
        get { return _terrainManager; }
        set { _terrainManager = value; }
    }

    public int HandSlotIndex
    {
        get { return _handSlotIndex; }
        set { _handSlotIndex = value; }
    }

    private void Start()
    {
        _selectedObject.SetActive(false);
        ChangeTerrainType(_terrainType);
    }

    private void ChangeTerrainType(TerrainType type)
    {
        _terrainType = type;
        _name = _terrainManager.GetTerrainName(type);
        _nameText.text = _name;
        _image.color = _terrainManager.GetTerrainColor(type);
    }

    public void OnClick()
    {
        _handSlots.ResetSelect();
        _selectedObject.SetActive(true);
        _isSelected = true;
    }

    public void UnSelect()
    {
        _isSelected = false;
        _selectedObject.SetActive(false);
    }
}
