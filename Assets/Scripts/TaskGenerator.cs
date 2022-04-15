using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface ITaskGenerator
{
    ItemData ItemToCreate { get; }
}

public class TaskGenerator : MonoBehaviour, ITaskGenerator
{
    public ItemData ItemToCreate => _itemToCreate;
    [SerializeField] private GameObject _itemSellerObject;
    [SerializeField] private Image _itemImage;
    [SerializeField] private Text _itemName;
    [SerializeField] private Text _itemPrice;
    [SerializeField] private float _tasksDelay;
    private IItemSeller _itemSeller;
    private ItemData _itemToCreate;

    private void Start()
    {
        _itemSeller = _itemSellerObject.GetComponent<IItemSeller>();
        _itemSeller.OnSellItem.AddListener(() => StartCoroutine(GenerateTaskByDelay()));
        GenerateTask();
    }

    private void GenerateTask()
    {
        int randomRecepieIndex = Random.Range(0, Recepies.RecepiesData.Count);
        _itemToCreate = Recepies.RecepiesData[randomRecepieIndex].ItemToCreate;
        _itemImage.sprite = _itemToCreate.Image;
        _itemName.text = _itemToCreate.Name;
        _itemPrice.text = _itemToCreate.Price.ToString();
    }

    private IEnumerator GenerateTaskByDelay()
    {
        yield return new WaitForSeconds(_tasksDelay);
        GenerateTask();
    }

    private void OnDestroy()
    {
        _itemSeller.OnSellItem.RemoveListener(() => StartCoroutine(GenerateTaskByDelay()));
    }
}
