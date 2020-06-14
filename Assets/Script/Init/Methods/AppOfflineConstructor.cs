using UnityEngine;
using InputControllers.Android;
using System;
using Map.Platforms;
using Unity.Mathematics;
using Object = UnityEngine.Object;

namespace Init.Methods
{
    public class AppOfflineConstructor : IObjectConstructor
    {
        private static PrefabManager _prefabManager;
        private bool _isUpperPlayer = false;
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
            var controller = playerObject.AddComponent<AppPlayerController>();
            var player = playerObject.GetComponent<Player>();
            player.InitPlayer(controller , iniState);
            
            var playerController = PrefabManager.GetInstance().GetGameObject("AppController");
            Vector3 controllerPosition = _isUpperPlayer ? new Vector3(0, 10.5f) : new Vector3(0, -10.5f);
            Object.Instantiate(playerController, controllerPosition, quaternion.identity);
            _isUpperPlayer = !_isUpperPlayer;
            
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
                    gameObject = _prefabManager.GetGameObject("DirectionPlatform");
                    break;
                case PlatformTypes.Fragile:
                    gameObject = _prefabManager.GetGameObject("FragilePlatform");
                    break;
                case PlatformTypes.Freeze:
                    gameObject = _prefabManager.GetGameObject("FreezePlatform");
                    break;
                case PlatformTypes.Normal:
                    gameObject = _prefabManager.GetGameObject("NormalPlatform");
                    break;
                case PlatformTypes.Spike:
                    gameObject = _prefabManager.GetGameObject("SpikePlatform");
                    break;
                case PlatformTypes.Time:
                    gameObject = _prefabManager.GetGameObject("TimePlatform");
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