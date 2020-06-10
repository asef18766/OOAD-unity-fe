using Map.Platforms;
using UnityEngine;
using System;
using InputControllers.Network.Server;
using Object = UnityEngine.Object;
using Network;
using SocketIO;
using Utils;
using UUID;


namespace Init.Methods
{
    public class ServerConstructor : IObjectConstructor
    {
        private static PrefabManager _prefabManager;
        private SocketIOComponent _network;
        public ServerConstructor()
        {
            _network = NetworkManager.GetInstance().GetComponent();
        }
        public Player PlayerConstructor(Vector3 pos, Vector2 scale, PlayerState iniState)
        {
            if (_prefabManager == null)
            {
                var pIns = PrefabManager.GetInstance();
                if (pIns == null)
                    throw new Exception("get null prefab manager");
                _prefabManager = pIns;
            }

            var playerPrefab = _prefabManager.GetGameObject("Player");
            if (playerPrefab == null)
                throw new Exception("get null player prefab");

            var playerObject = Object.Instantiate(playerPrefab);
            playerObject.transform.position = pos;
            playerObject.transform.localScale = scale;
            var controller = playerObject.AddComponent<OperationEventListener>();
            var player = playerObject.GetComponent<Player>();
            player.InitPlayer(controller, iniState);
            player.ModifySelfId(Guid.NewGuid());
            _sendInit(playerObject, "Player");

            return player;
        }

        public IPlatform PlatformConstructor(Vector3 pos, Vector2 scale, PlatformTypes type)
        {
            if (_prefabManager == null)
            {
                var pIns = PrefabManager.GetInstance();
                if (pIns == null)
                    throw new Exception("get null prefab manager");
                _prefabManager = pIns;
            }
            string platform;

            switch (type)
            {
                case PlatformTypes.Direction:
                    platform = "DirectionPlatform";
                    break;
                case PlatformTypes.Fragile:
                    platform = "FragilePlatform";
                    break;
                case PlatformTypes.Freeze:
                    platform = "FreezePlatform";
                    break;
                case PlatformTypes.Normal:
                    platform = "NormalPlatform";
                    break;
                case PlatformTypes.Spike:
                    platform = "SpikePlatform";
                    break;
                case PlatformTypes.Time:
                    platform = "TimePlatform";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            GameObject gameObject = _prefabManager.GetGameObject(platform);
            var instance = Object.Instantiate(gameObject);
            instance.transform.position = pos;
            instance.transform.localScale = scale;
            _sendInit(instance, platform);
            
            return instance.GetComponent<IPlatform>();
        }

        private void _sendInit(GameObject obj, string name)
        {
            var jsonObject = new JSONObject($"{{\"type\":\"Instantiate\"}}");
            jsonObject["args"] = new JSONObject($"{{\"uuid\":\"{obj.GetComponent<UuidObject>().uuid.ToString()}\",\"parent\":null,\"prefab\":\"{name}\"}}");
            jsonObject["args"]["transform"] = new JSONObject();
            jsonObject["args"]["transform"]["position"] = Jsonify.VectortoJson(obj.transform.position);
            jsonObject["args"]["transform"]["rotation"] = Jsonify.VectortoJson(obj.transform.rotation.eulerAngles);
            _network.Emit("updateEntity", jsonObject);
        }
    }
}