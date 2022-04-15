using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface IItemSeller
{
    UnityEvent OnSellItem { get; }

    void EnableSellButton();
    void SellItem();
    void UpdateItemPrice(Item item, bool itemToSell);
}

public class ItemSeller : MonoBehaviour, IItemSeller
{
    public UnityEvent OnSellItem => _onSellItem;
    [SerializeField] private int _priceDenominator;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Text _price;
    [SerializeField] private GameObject _moneyObject;
    [SerializeField] private GameObject _itemSeparatorObject;
    private readonly UnityEvent _onSellItem = new UnityEvent();
    private IItemSeparator _itemsSeparator;
    private IMoney _money;

    private void Start()
    {
        _itemsSeparator = _itemSeparatorObject.GetComponent<IItemSeparator>();
        _money = _moneyObject.GetComponent<IMoney>();
        DisableSellButton();
    }

    public void EnableSellButton()
    {
        _sellButton.interactable = true;
    }

    private void DisableSellButton()
    {
        _sellButton.interactable = false;
    }

    public void UpdateItemPrice(Item item, bool itemToSell)
    {
        item.Price = item.ItemData.Price;
        if (!itemToSell)
        {
            item.Price = (item.Price / _priceDenominator);
            _price.text = item.Price.ToString();
        }
    }

    public void SellItem()
    {
        int price = int.Parse(_price.text);
        _money.AddMoney(price);
        DisableSellButton();
        Item itemToRemove = _itemsSeparator.CreateParent.GetComponentInChildren<Item>();
        _itemsSeparator.ItemsToSeparate.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
        OnSellItem.Invoke();
    }
}

