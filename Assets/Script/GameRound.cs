using System;
using System.Collections;
using Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameRound : MonoBehaviour
{
    public static GameRound Instance;
    [SerializeField] private float roundTime = 10.2f;
    [SerializeField] private float updateRate = 0.9f;

    private static readonly JSONObject UpdateSwapTimeFormat = new JSONObject("{\"time\":87}");
    private float RemainTime
    {
        set
        {
            _remainTime = value;
            if(_remainTime <= 0)
            {
                _remainTime = roundTime;
                _eventManager.InvokeEvent("swap" , JSONObject.nullJO);
            }
            var arg = UpdateSwapTimeFormat.Copy();
            arg["time"].n = _remainTime;
            _eventManager.InvokeEvent("updateSwapTime" , arg);
        }
        get => _remainTime;
    }
    private float _remainTime = 10.2f;
    private EventManager _eventManager;
    private IEnumerator _minusTime(float amount)
    {
        RemainTime -= amount;
        yield break;
    }
    private IEnumerator _swapRound()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(updateRate);
            RemainTime -= updateRate;
        }
    }

    private bool _isGameEnded = false;
    public void EndGame(string loser)
    {
        if(_isGameEnded) return;
        CoroutineRunner.Runner.StopAllCoroutines();
        print("game ended");
        _eventManager.InvokeEvent("endGame" , JSONObject.nullJO);
        StopAllCoroutines();
        GameChoice.Winner = (loser == "p1") ? "p2" : "p1";

        switch (GameChoice.GameMode)
        {
            case GameMode.Offline:
                SceneManager.LoadScene("EndScence");
                break;
            case GameMode.Online:
                break;
            case GameMode.Server:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        EventManager.GetInstance().Clean();
    }
    private void Start()
    {
        _eventManager = EventManager.GetInstance();
        Instance = this;
        RemainTime = roundTime;
        StartCoroutine(_swapRound());
    }
}