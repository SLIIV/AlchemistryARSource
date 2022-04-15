using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 0)]
public class ItemData : ScriptableObject
{
    public string Name => _name;
    public GameObject Prefab => _prefab;
    public Sprite Image => _image;
    public int Price => _price;
    public ItemType Type => _type;
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite _image;
    [SerializeField] private int _price;
    [SerializeField] private Color[] _temperatureColors;
    [SerializeField] private ItemType _type;

    public static ItemData operator + (ItemData a, ItemData b)
    {
        for (int i = 0; i < Recepies.RecepiesData.Count; i++)
        {
            if (IsRecepiesContainItems(i, a, b))
            {
                if (a.IsTemperaturesMatched(i) && b.IsTemperaturesMatched(i))
                {
                    return Recepies.RecepiesData[i].ItemToCreate;
                }
            }
        }
        return null;
    }

    public Color GetColorByTemperature(Item.TemperatureStatus temperature)
    {
        return _temperatureColors[(int)temperature];
    }

    private static bool IsRecepiesContainItems(int i, ItemData a, ItemData b)
    {
        List<RecepieData.ItemConsistOf> itemCosistOf = Recepies.RecepiesData[i].ItemsConsistOf;
        if (itemCosistOf.Contains(itemCosistOf.Find(x => x.Item.Name == a.Name)) &&
            itemCosistOf.Contains(itemCosistOf.Find(y => y.Item.Name == b.Name)))
        {
            return true;
        }
        return false;
    }

    private bool IsRecepiesContainItem()
    {

        for (int j = 0; j < Recepies.RecepiesData.Count; j++)
        {
            List<RecepieData.ItemConsistOf> itemConsist = Recepies.RecepiesData[j].ItemsConsistOf;
            if (itemConsist.Contains(itemConsist.Find(a => a.Item.Name == Name)))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsTemperaturesMatched(int i)
    {
        RecepieData itemConsist = Recepies.RecepiesData[i];
        Item item = Recepies.ItemSeparator.ItemsToSeparate.Find(a => a.ItemData.Name == Name);
        if(item.Temperature == itemConsist.ItemsConsistOf.Find(b => b.Item.Name == Name).temperature)
        {
            return true;
        }
        return false;
    }

    public enum ItemType
    {
        ITEM,
        HEATER,
        COOLER,
        BAD
    }
}
