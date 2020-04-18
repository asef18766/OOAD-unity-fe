using UnityEngine;
using InputControllers.Pc;
using System;
namespace Init.Methods
{
    class PCInit
    {
        public static void BuildPc()
        {
            var prefabManager = PrefabManager.GetInstance();
            if(prefabManager == null)
                throw new Exception("get null prefab manager");
            
            var playerPrefab = prefabManager.GetGameObject("Player");
            if(playerPrefab == null)
                throw new Exception("get null player prefab");
            
            if (Camera.main != null)
            {
                var playerObject = MonoBehaviour.Instantiate(playerPrefab);
                playerObject.transform.position = Vector3.zero;
                var controller = playerObject.AddComponent<PcPlayerController>();
                var player = playerObject.GetComponent<Player>();
                player.InitPlayer(controller , PlayerState.Jump);
            }
            else
                throw new ArgumentException("can not found main camera during initialization");
            
        }
    }
}