using UnityEngine;
using UnityEngine.UI;

public class RecepiesBookGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _recepiePrefab;
    [SerializeField] private Transform _recepiesParent;
    [SerializeField] private GridLayoutGroup _group;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = _recepiesParent.GetComponent<RectTransform>();
        GenerateRecepies();
    }

    private void GenerateRecepies()
    {
        for(int i = 0; i < Recepies.RecepiesData.Count; i++)
        {
            GameObject recepieObject = Instantiate(_recepiePrefab, _recepiesParent);
            IRecepiesInBook recepies = recepieObject.GetComponent<IRecepiesInBook>();
            for(int j = 0; j < recepies.Recepie.Length; j++)
            {
                recepies.Recepie[j].Image.sprite = Recepies.RecepiesData[i].ItemsConsistOf[j].Item.Image;
                recepies.Recepie[j].Temperature.text = Recepies.RecepiesData[i].ItemsConsistOf[j].temperature.ToString();
            }
            recepies.ResultImage.sprite = Recepies.RecepiesData[i].ItemToCreate.Image;
            _rectTransform.sizeDelta += new Vector2(0, _group.cellSize.y);
        }
    }
}
