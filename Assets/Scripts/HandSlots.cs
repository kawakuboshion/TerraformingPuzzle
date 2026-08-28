using System.Collections.Generic;
using UnityEngine;

public class HandSlots : MonoBehaviour
{
    [SerializeField] private TerrainManager _manager;
    [SerializeField] private TerrainCard _terrainCardPrefab;
    [SerializeField] private CardConfig _cardConfig;
    [SerializeField] private CardGenerator _cardGenerator;

    private List<TerrainCard> _terrainCards = new();
    private int _selectIndex = 0;

    void Start()
    {
        _cardGenerator.Initialize(_cardConfig);

        for (int i = 0; i < _cardConfig.SlotCount; i++)
        {
            SetNewCard(i);
        }
    }

    public int GetCardCount() => _terrainCards.Count;

    public void ResetSelect()
    {
        for (int i = 0; i < _terrainCards.Count; i++)
        {
            _terrainCards[i].UnSelect();
        }
    }

    public bool TryGetUseCard(out TerrainType terrainType)
    {
        if (TryGetSelectIndex(out _selectIndex))
        {
            terrainType = _terrainCards[_selectIndex].TerrainType;
            return true;
        }
        terrainType = TerrainType.VacantLot;
        return false;
    }

    public void UseCard()
    {
        var card = _terrainCards[_selectIndex].gameObject;
        _terrainCards.Remove(_terrainCards[_selectIndex]);
        Destroy(card);
        SetNewCard(_selectIndex);
    }

    private bool TryGetSelectIndex(out int index)
    {
        for (int i = 0; i < _terrainCards.Count; i++)
        {
            if (_terrainCards[i].IsSelected)
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
        TerrainCard card = Instantiate(_terrainCardPrefab, transform);
        card.TerrainManager = _manager;
        card.TerrainType = _cardGenerator.GetRandomTerrainType();
        card.HandSlots = this;
        card.HandSlotIndex = index;
        _terrainCards.Add(card);
    }
}
