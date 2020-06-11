using Event;
using UnityEngine;

namespace Utils
{
    public class CoroutineRunner : MonoBehaviour
    {
        public static CoroutineRunner Runner = null;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Runner = this;
            EventManager.GetInstance().RegisterEvent("endGame" , (s, o) => StopAllCoroutines());
        }
        
    }
}