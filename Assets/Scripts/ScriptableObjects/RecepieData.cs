using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recepie", menuName = "ScriptableObjects/Recepie", order = 1)]
public class RecepieData : ScriptableObject
{
    public ItemData ItemToCreate;
    public List<ItemConsistOf> ItemsConsistOf = new List<ItemConsistOf>();

    [System.Serializable]
    public struct ItemConsistOf
    {
        public ItemData Item;
        public Item.TemperatureStatus temperature;
    }
}
