using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ITimer
{
    UnityEvent OnTimerEnd { get; }
}

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour, ITimer
{
    public UnityEvent OnTimerEnd => _onTimerEnd;
    [SerializeField] private float _startTime;
    private Text _timer;
    private float _currentTime;
    private readonly UnityEvent _onTimerEnd = new UnityEvent();

    private void Start()
    {
        _timer = GetComponent<Text>();
        _currentTime = _startTime;
        _timer.text = SecondsToTimer(_currentTime);
        StartCoroutine(StartTimer());
    }

    private string SecondsToTimer(float seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    }

    private IEnumerator StartTimer()
    {
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _currentTime -= 1f;
            _timer.text = SecondsToTimer(_currentTime);
        }
        _onTimerEnd.Invoke();
        yield break;
    }
}
