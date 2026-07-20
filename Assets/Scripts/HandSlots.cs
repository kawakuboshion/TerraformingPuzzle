using UnityEngine;

public class HandSlots : MonoBehaviour
{
    [SerializeField] private TerrainManager _manager;
    [SerializeField] private TerrainCard _terrainCardPrefab;
    private TerrainCard[] _terrainCards = new TerrainCard[3];
    private int _selectIndex = 0;
    void Start()
    {
        for (int i = 0; i < _terrainCards.Length; i++)
        {
            SetNewCard(i);
        }
    }

    public void ResetSelect()
    {
        for (int i = 0; i < _terrainCards.Length; i++)
        {
            _terrainCards[i].UnSelect();
        }
    }

    public bool TryGetUseCard(out TerrainType terrainType)
    {
        if(TryGetSelectIndex(out _selectIndex))
        {
            terrainType = _terrainCards[_selectIndex]._TerainType;
            return true;
        }
        terrainType = TerrainType.VacantLot;
        return false;
    }

    private bool TryGetSelectIndex(out int index)
    {
        for (int i = 0; i < _terrainCards.Length; i++)
        {
            if (_terrainCards[i]._isSelected)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public void SetNewCard(int index)
    {
        TerrainCard card = Instantiate(_terrainCardPrefab,transform);
        card._terrainManager = _manager;
        card._TerainType = (TerrainType)Random.Range((int)TerrainType.Water, (int)TerrainType.Mud);
        card._handSlots = this;
        card._handSlotIndex = index;
        _terrainCards[index] = card;
    }
}
