using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] private GameObject _victoryWindow;
    [SerializeField] private GameObject _gameRules;
    private IVictory _victory;

    private void Start()
    {
        _victory = _gameRules.GetComponent<IVictory>();
        _victory.OnVictory.AddListener(() => ShowVictoryWindow());
    }

    private void ShowVictoryWindow()
    {
        _victoryWindow.SetActive(true);
    }

    private void OnDestroy()
    {
        _victory.OnVictory.RemoveListener(() => ShowVictoryWindow());
    }
}
