using Event;
using UnityEngine;

namespace ThreadUtils
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