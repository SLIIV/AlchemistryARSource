using System.Collections.Generic;
using UnityEngine;

public interface IItemSeparator
{
    List<Item> ItemsToSeparate { get; }
    Transform CreateParent { get; }
    bool ItemInResult { get; }
}

public class ItemSeparator : MonoBehaviour, IItemSeparator
{
    public List<Item> ItemsToSeparate => _itemsToSeparate;
    public Transform CreateParent => _createParent;
    public bool ItemInResult => ItemInResult;
    private const int FIRST_ITEM = 0;
    private const int SECOND_ITEM = 1;
    private const int THIRD_ITEM = 2;
    private const int ITEMS_TO_SEPARATE = 2;
    private const int MAX_ITEMS_IN_SEPARATE = 3;
    [SerializeField] private GameObject _failedItemPrefab;
    [SerializeField] private Transform _createParent;
    [SerializeField] private float _distanceToSeparate;
    [SerializeField] private GameObject _itemSellerObject;
    [SerializeField] private float _timeToChangeTemperatureStatus;
    [SerializeField] private GameObject _taskGeneratorObject;
    [SerializeField] private float _distanceToActivateItems;
    private float _currentTimeToChangeTemperature;
    private List<Item> _itemsToSeparate = new List<Item>();
    private IItemSeller _itemSeller;
    private ITaskGenerator _taskGenerator;
    private bool _itemCreated;
    private List<Item> _resultItems = new List<Item>();
    
    private void Start()
    {
        _taskGenerator = _taskGeneratorObject.GetComponent<ITaskGenerator>();
        _itemSeller = _itemSellerObject.GetComponent<IItemSeller>();
    }

    private void Update()
    {
        if (_itemsToSeparate.Count == ITEMS_TO_SEPARATE || (_itemsToSeparate.Count == MAX_ITEMS_IN_SEPARATE && _itemCreated))
        {
            if (DistanceBetweenItems() >= _distanceToActivateItems && ItemsAreUsed(FIRST_ITEM, SECOND_ITEM))
            {
                for (int i = 0; i < _itemsToSeparate.Count; i++)
                {
                    _itemsToSeparate[i].Used = false;
                }
            }

            if (IsHeader(out Item heater))
            {
                if (DistanceBetweenItemsLessToSeparate(heater, out Item itemToChange))
                    HeatUp(itemToChange);
            }
            else if (IsCooler(out Item cooler))
            {
                if (DistanceBetweenItemsLessToSeparate(cooler, out Item itemToChange))
                    CoolDown(itemToChange);
            }
            else if (DistanceBetweenItems() <= _distanceToSeparate)
            {
                if (_itemsToSeparate.Count == ITEMS_TO_SEPARATE && !ItemsAreUsed(FIRST_ITEM, SECOND_ITEM))
                {
                    if (_itemsToSeparate[FIRST_ITEM].ItemData.Type != ItemData.ItemType.BAD && _itemsToSeparate[SECOND_ITEM].ItemData.Type != ItemData.ItemType.BAD)
                        SeparateItems(FIRST_ITEM, SECOND_ITEM);
                }
                else if (_itemsToSeparate.Count == MAX_ITEMS_IN_SEPARATE && !ItemsAreUsed(SECOND_ITEM, THIRD_ITEM))
                {
                    if (_itemsToSeparate[SECOND_ITEM].ItemData.Type != ItemData.ItemType.BAD && _itemsToSeparate[THIRD_ITEM].ItemData.Type != ItemData.ItemType.BAD)
                    {
                        SeparateItems(SECOND_ITEM, THIRD_ITEM);
                    }
                }
            }
            else
            {
                if (_currentTimeToChangeTemperature != 0)
                {
                    _currentTimeToChangeTemperature = 0;
                }
            }
        }
    }

    private void SeparateItems(int itemIndexA, int itemIndexB)
    {
        ItemData itemData = _itemsToSeparate[itemIndexA].ItemData + _itemsToSeparate[itemIndexB].ItemData;
        Item item;
        for (int i = _itemsToSeparate.Count - 1; i >= 0; i--)
        {
            _itemsToSeparate[i].Used = true;
            Item itemToRemove = _itemsToSeparate[i];
            _itemsToSeparate.Remove(itemToRemove);
            if (_resultItems.Contains(itemToRemove))
            {
                _resultItems.Remove(itemToRemove);
                Destroy(itemToRemove.gameObject);
            }
        }

        if (itemData != null)
        {
            GameObject itemObject = Instantiate(itemData.Prefab, _createParent);
            item = itemObject.GetComponent<Item>();
            _itemsToSeparate.Add(item);
            if(_taskGenerator.ItemToCreate == itemData)
                _itemSeller.UpdateItemPrice(item, true);
            else
                _itemSeller.UpdateItemPrice(item, false);
        }
        else
        {
            GameObject itemObject = Instantiate(_failedItemPrefab, _createParent);
            item = itemObject.GetComponent<Item>();
            _itemSeller.UpdateItemPrice(item, false);
            _itemsToSeparate.Add(item);
        }
        _itemCreated = true;
        _resultItems.Add(item);
        _itemSeller.EnableSellButton();
    }

    private void HeatUp(Item item)
    {
        if (_currentTimeToChangeTemperature < _timeToChangeTemperatureStatus)
        {
            _currentTimeToChangeTemperature += Time.deltaTime;
        }
        else
        {
            if (item.Temperature < Item.TemperatureStatus.HOT)
            {
                item.Temperature++;
                item.ChangeColorByTemperature(item.Temperature);
            }
            _currentTimeToChangeTemperature = 0;
        }
    }

    private void CoolDown(Item item)
    {
        if (_currentTimeToChangeTemperature < _timeToChangeTemperatureStatus)
        {
            _currentTimeToChangeTemperature += Time.deltaTime;
        }
        else
        {
            if (item.Temperature > Item.TemperatureStatus.COLD)
            {
                item.Temperature--;
                item.ChangeColorByTemperature(item.Temperature);
            }
            _currentTimeToChangeTemperature = 0;
        }
    }

    private bool IsHeader(out Item heater)
    {
        if (heater =_itemsToSeparate.Find(a => a.ItemData.Type == ItemData.ItemType.HEATER))
            return true;
        return false;
    }

    private bool IsCooler(out Item cooler)
    {
        if (cooler = _itemsToSeparate.Find(a => a.ItemData.Type == ItemData.ItemType.COOLER))
            return true;
        return false;
    }

    public void AddItemToSeparate(Item item)
    {
        if (_itemsToSeparate.Count < ITEMS_TO_SEPARATE || (_itemsToSeparate.Count < MAX_ITEMS_IN_SEPARATE && _itemCreated))
        {
            _itemsToSeparate.Add(item);
        }
    }

    public void RemoveItemFromList(Item item)
    {
        _itemsToSeparate.Remove(item);
    }

    private float DistanceBetweenItems()
    {
        if (_itemsToSeparate.Count == 3)
        {
            return Vector3.Distance(_itemsToSeparate[SECOND_ITEM].transform.position, _itemsToSeparate[THIRD_ITEM].transform.position);
        }
        return Vector3.Distance(_itemsToSeparate[FIRST_ITEM].transform.position, _itemsToSeparate[SECOND_ITEM].transform.position);
    }

    private bool DistanceBetweenItemsLessToSeparate(Item temperatureItem, out Item itemToChange)
    {
        List<Item> items = _itemsToSeparate.FindAll(a => a.ItemData.Type == ItemData.ItemType.ITEM);
        for(int i = 0; i < items.Count; i++)
        {
            if(Vector2.Distance(temperatureItem.transform.position, items[i].transform.position) <= _distanceToSeparate)
            {
                itemToChange = items[i];
                return true;
            }
        }
        itemToChange = null;
        return false;
    }

    private bool ItemsAreUsed(int itemA, int ItemB)
    {
        return _itemsToSeparate[itemA].Used || _itemsToSeparate[ItemB].Used;
    }
}
