using UnityEngine;
using System;
using InputControllers.Network.Client;
using Map.Platforms;
using Object = UnityEngine.Object;

namespace Init.Methods
{
    public class OnlineClientConstructor : IObjectConstructor
    {
        private static PrefabManager _prefabManager;
        public Player PlayerConstructor(Vector3 pos, Vector2 scale, PlayerState iniState)
        {
            var playerObject = new GameObject();
            playerObject.AddComponent<OnlinePlayerClientController>().Init();
            return null;
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
            GameObject gameObject;
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