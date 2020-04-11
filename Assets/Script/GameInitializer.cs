using System;
using InputControllers.Pc;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameInitializer : MonoBehaviour
{
    private static void _buildWindows()
    {
        var prefabManager = PrefabManager.GetInstance();
        var playerPrefab = prefabManager.GetGameObject("Player");
        if (Camera.main != null)
        {
            var playerObject = Instantiate(playerPrefab);
            playerObject.transform.position = Vector3.zero;
            var controller = playerObject.AddComponent<PcPlayerController>();
            var player = playerObject.GetComponent<Player>();
            player.InitPlayer(controller , PlayerState.Jump);
        }
        else
            throw new ArgumentException("can not found main camera during initialization");
        
    }
    private void Awake()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                break;
            case RuntimePlatform.LinuxPlayer:
                break;
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                _buildWindows();
                break;
            case RuntimePlatform.OSXPlayer:
                break;
            default:
                throw new ArgumentException($"does not support at platform {Application.platform}");
        }
    }
    void Update()
    {
        print("hello");
    }
}