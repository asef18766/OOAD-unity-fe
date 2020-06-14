using UnityEngine;
using Event;
using UnityEngine.UI;
namespace UI.MainGame
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject swapAni;
        [SerializeField] private Slider[] healthBars;
        [SerializeField] private Text remainSwapTimeDisplay;
        private void Awake()
        {
            var eventManager=EventManager.GetInstance();
            eventManager.RegisterEvent("swap" , swapAnimation);
            eventManager.RegisterEvent("playerHurt", playerHurt);
            eventManager.RegisterEvent("updateSwapTime", updateSwapTime);
        }
        void swapAnimation(string ev,JSONObject obj)
        {
            swapAni.SetActive(true);
        }
        void playerHurt(string ev,JSONObject obj)
        {
            var playerName = obj["playerName"].str;
            var health = obj["health"].n / 100;
            print($"set {playerName} health bar: {health}");
            healthBars[(playerName  == "p1")? 0:1].value = health;
        }
        void updateSwapTime(string ev,JSONObject obj)
        {
            print($"update swap time to {obj["time"].n}");
            if (remainSwapTimeDisplay == null)
            {
                Debug.Log("text bar is null");
                return;
            }
            remainSwapTimeDisplay.text = obj["time"].n.ToString("#0.0");
        }
    }
}
