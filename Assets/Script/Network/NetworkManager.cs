using SocketIO;
using UnityEngine;

namespace Network
{
    public class NetworkManager
    {
        private static SocketIOComponent _instance = null;
        private static NetworkManager _self = null;
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

        public SocketIOComponent GetComponent() => _instance;

        public void Clean()
        {
            if(_instance == null)
                return;
            
            var ip = _instance.url;
            Object.Destroy(_instance.gameObject);
            
            var pref = PrefabManager.GetInstance().GetGameObject("Network");
            var gameObject = Object.Instantiate(pref);
            Object.DontDestroyOnLoad(gameObject);
            _instance = gameObject.GetComponent<SocketIOComponent>();
        }
    }
}