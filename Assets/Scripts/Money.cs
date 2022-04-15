using UnityEngine;
using UnityEngine.UI;

public interface IMoney
{
    int Count { get; }

    void AddMoney(int value);
}

[RequireComponent(typeof(Text))]
public class Money : MonoBehaviour, IMoney
{
    public int Count => _count;
    private Text _moneyText;
    private int _count;

    private void Start()
    {
        _moneyText = GetComponent<Text>();
        _moneyText.text = "0";
    }

    public void AddMoney(int value)
    {
        _count += value;
        _moneyText.text = _count.ToString();
    }
}
