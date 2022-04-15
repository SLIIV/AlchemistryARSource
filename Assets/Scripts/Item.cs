using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public ItemData ItemData;
    public TemperatureStatus Temperature;
    public bool Used;
    public int Price;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void ChangeColorByTemperature(TemperatureStatus temperatureStatus)
    {
        _sprite.color = ItemData.GetColorByTemperature(temperatureStatus);
    }

    public enum TemperatureStatus
    {
        COLD,
        WARM,
        HOT
    }
}
