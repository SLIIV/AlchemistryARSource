using UnityEngine;

public class Lose : MonoBehaviour
{
    [SerializeField] private GameObject _loseWindow;
    [SerializeField] private GameObject _gameRulesObject;
    private ILosable _lose;

    private void Start()
    {
        _lose = _gameRulesObject.GetComponent<ILosable>();
        _lose.OnLose.AddListener(() => ShowLoseWindow());
    }

    private void ShowLoseWindow()
    {
        _loseWindow.SetActive(true);
    }

    private void OnDestroy()
    {
        _lose.OnLose.RemoveListener(() => ShowLoseWindow());
    }
}
