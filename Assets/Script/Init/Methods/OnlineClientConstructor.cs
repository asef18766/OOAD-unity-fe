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
            throw new NotImplementedException();
        }
    }
}