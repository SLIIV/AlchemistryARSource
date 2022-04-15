using System.Collections.Generic;
using UnityEngine;

public class Recepies : MonoBehaviour
{
    public static List<RecepieData> RecepiesData => _recepiesDataStatic;
    public static IItemSeparator ItemSeparator => _itemSeparatorStatic;
    [SerializeField] private List<RecepieData> _recepiesData;
    [SerializeField] private GameObject _itemSeparatorObject;
    private static IItemSeparator _itemSeparatorStatic;
    private static List<RecepieData> _recepiesDataStatic;

    private void Awake()
    {
        _recepiesDataStatic = _recepiesData;
        _itemSeparatorStatic = _itemSeparatorObject.GetComponent<IItemSeparator>();
    }
}
