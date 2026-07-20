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
    public bool _isSelected = false; 
    public TerrainType _TerainType { get { return _terrainType; } set { ChangeTerrainType(value); } }
    public HandSlots _handSlots { private get; set; }
    public TerrainManager _terrainManager { private get; set; }
    public int _handSlotIndex { private get; set; }

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

    public void UnSelect ()
    {
        _isSelected = false;
        _selectedObject.SetActive(false);
    }
}
