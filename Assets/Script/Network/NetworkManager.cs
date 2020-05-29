using SocketIO;
using UnityEngine;

namespace Network
{
    public class NetworkManager
    {
        private static SocketIOComponent _instance;
        private static NetworkManager _self;
        private NetworkManager()
        {
            if (_instance != null)
                return;
            var pref = PrefabManager.GetInstance().GetGameObject("Network");
            var gameObject = Object.Instantiate(pref);
            Object.DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<SocketIOComponent>();
            _self = this;
        }

        public static NetworkManager GetInstance()
        {
            return _self ?? (_self = new NetworkManager());
        }

        public static bool HasInstance() => _instance != null;

        public SocketIOComponent GetComponent() => _instance;

        public void Clean()
        {
            if(_instance == null)
                return;
            Object.Destroy(_instance.gameObject);
            
            var pref = PrefabManager.GetInstance().GetGameObject("Network");
            var gameObject = Object.Instantiate(pref);
            Object.DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<SocketIOComponent>();
        }
    }
}