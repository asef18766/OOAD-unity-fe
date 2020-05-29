using UnityEngine;
using InputControllers;
using System;
using Map.Platforms;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Init.Methods
{
    public class PcOnlineConstructor : IObjectConstructor
    {
        private static PrefabManager _prefabManager;
        public Player PlayerConstructor(Vector3 pos, Vector2 scale, PlayerState iniState)
        {
            if (_prefabManager == null)
            {
                var pIns = PrefabManager.GetInstance();
                if(pIns == null)
                    throw new Exception("get null prefab manager");
                _prefabManager = pIns;
            }

            var playerPrefab = _prefabManager.GetGameObject("Player");
            if(playerPrefab == null)
                throw new Exception("get null player prefab");
            
            var playerObject = Object.Instantiate(playerPrefab);
            playerObject.transform.position = pos;
            playerObject.transform.localScale = scale;
            var controller = playerObject.AddComponent<OnlinePlayerController>();
            controller.Init("pc");
            var player = playerObject.GetComponent<Player>();
            player.InitPlayer(controller , iniState);
            return player;
        }

        public IPlatform PlatformConstructor(Vector3 pos, Vector2 scale, PlatformTypes type)
        {
            if (_prefabManager == null)
            {
                var pIns = PrefabManager.GetInstance();
                if(pIns == null)
                    throw new Exception("get null prefab manager");
                _prefabManager = pIns;
            }
            GameObject gameObject=null;
            switch (type)
            {
                case PlatformTypes.Direction:
                    throw new NotImplementedException();
                    break;
                case PlatformTypes.Fragile:
                    throw new NotImplementedException();
                    break;
                case PlatformTypes.Freeze:
                    throw new NotImplementedException();
                    break;
                case PlatformTypes.Normal:
                    gameObject = _prefabManager.GetGameObject("NormalPlatform");
                    break;
                case PlatformTypes.Spike:
                    throw new NotImplementedException();
                    break;
                case PlatformTypes.Time:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var instance = Object.Instantiate(gameObject);
            instance.transform.position = pos;
            instance.transform.localScale = scale;
            return instance.GetComponent<IPlatform>();
        }
    }
}