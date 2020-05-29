using System.Collections;
using Event;
using UnityEngine;

public class GameRound : MonoBehaviour
{
    public static GameRound Instance;
    [SerializeField] private float roundTime = 10.2f;
    [SerializeField] private float updateRate = 0.9f;
    private float _remainTime;
    private EventManager _eventManager;
    
    private IEnumerator _swapRound()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateRate);
            _remainTime -= updateRate;
            if (!(_remainTime <= 0.0f)) continue;
            _eventManager.InvokeEvent("swap" , JSONObject.nullJO);
            _remainTime = roundTime;
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
        _remainTime = roundTime;
        StartCoroutine(_swapRound());
    }
}