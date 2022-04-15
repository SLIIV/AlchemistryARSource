using UnityEngine;
using UnityEngine.UI;

public interface IRecepiesInBook
{
    RecepiesInBook.RecepieInBook[] Recepie { get; }
    Image ResultImage { get; }
}

public class RecepiesInBook : MonoBehaviour, IRecepiesInBook
{
    public RecepieInBook[] Recepie => _recepie;
    public Image ResultImage => _resultImage;
    [SerializeField] private RecepieInBook[] _recepie;
    [SerializeField] private Image _resultImage;

    [System.Serializable]
    public struct RecepieInBook
    {
        public Image Image;
        public Text Temperature;
    }
}
