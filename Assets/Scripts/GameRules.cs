using UnityEngine;
using UnityEngine.Events;

public interface ILosable
{
    UnityEvent OnLose { get; }
}
public interface IVictory
{
    UnityEvent OnVictory { get; }
}

public class GameRules : MonoBehaviour, ILosable, IVictory
{
    public UnityEvent OnVictory => _onVictory;
    public UnityEvent OnLose => _onLose;
    [SerializeField] private int moneyToWin;
    [SerializeField] private GameObject _moneyObject;
    [SerializeField] private GameObject _timerObject;
    private readonly UnityEvent _onVictory = new UnityEvent();
    private readonly UnityEvent _onLose = new UnityEvent();
    private IMoney _money;
    private ITimer _timer;

    private void Start()
    {
        _money = _moneyObject.GetComponent<IMoney>();
        _timer = _timerObject.GetComponent<ITimer>();
        _timer.OnTimerEnd.AddListener(() => CheckForVictory());
    }

    private void CheckForVictory()
    {
        if (_money.Count >= moneyToWin)
        {
            _onVictory.Invoke();
        }
        else
        {
            _onLose.Invoke();
        }
    }

    private void OnDestroy()
    {
        _timer.OnTimerEnd.RemoveListener(() => CheckForVictory());
    }
}
