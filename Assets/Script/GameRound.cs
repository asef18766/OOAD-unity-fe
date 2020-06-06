using System.Collections;
using Event;
using UnityEngine;

public class GameRound : MonoBehaviour
{
    public static GameRound Instance;
    [SerializeField] private float roundTime = 10.2f;
    [SerializeField] private float updateRate = 0.9f;

    private static readonly JSONObject updateSwapTimeFormat = new JSONObject("{\"time\":87}");
    private float remainTime
    {
        set
        {
            _remainTime = value;
            if(_remainTime <= 0)
            {
                _remainTime = roundTime;
                _eventManager.InvokeEvent("swap" , JSONObject.nullJO);
            }
            var arg = updateSwapTimeFormat.Copy();
            arg["time"].n = _remainTime;
            _eventManager.InvokeEvent("updateSwapTime" , arg);
        }
        get
        {
            return _remainTime;
        }
    }
    private float _remainTime = 10.2f;
    private EventManager _eventManager;
    private IEnumerator _minusTime(float amount)
    {
        remainTime = remainTime - amount;
        yield break;
    }
    private IEnumerator _swapRound()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(updateRate);
            remainTime = remainTime - updateRate;
        }
    }

    public void EndGame()
    {
        print("game ended");
        _eventManager.InvokeEvent("endGame" , JSONObject.nullJO);
        StopAllCoroutines();
    }
    private void Start()
    {
        _eventManager = EventManager.GetInstance();
        Instance = this;
        remainTime = roundTime;
        StartCoroutine(_swapRound());
    }
}