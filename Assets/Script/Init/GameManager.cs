using System;
using Init.Methods;
using InputControllers.Pc;
using Map;
using Map.Platforms;
using UnityEngine;
namespace Init
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public IObjectConstructor creator;

        #region init_implementation

        private void _buildPcOffline()
        {
            var locations = new[]
            {
                new Vector2(1.75f , 3.45f),
                new Vector2(4.78f , 2.02f),
                new Vector2(-2.04f , 1.34f),
                new Vector2(0.42f , -0.4f),
                new Vector2(-2.07f , -2.43f),
                new Vector2(4.49f , -2.43f)
            };
            MapFactory.GetInstance();
            MapFactory.PlatformScale = 2;
            foreach (var location in locations)
                creator.PlatformConstructor(location, Vector2.one * 2,PlatformTypes.Normal);

            const int pScale = 4;
            creator.PlayerConstructor(new Vector2(-2.78f, 2.48f),Vector2.one * pScale, PlayerState.Jump);
            var p2 = creator.PlayerConstructor(new Vector2(3.904f, -3.963f), Vector2.one * pScale, PlayerState.Attack);
            p2.GetComponent<PcKeyboardModel>().walk = new[]
            {
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.LeftArrow,
                KeyCode.RightArrow
            };
        }

        #endregion
        private void Awake()
        {
            Instance = this;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    break;
                case RuntimePlatform.LinuxPlayer:
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    if (GameChoice.Gamemode == GameMode.Offline)
                    {
                        creator = new PcOfflineConstructor();
                        _buildPcOffline();
                    }
                    else
                    {
                        creator = new PcOnlineConstructor();
                        _buildPcOffline();
                    }
                    break;
                case RuntimePlatform.OSXPlayer:
                    break;
                default:
                    throw new ArgumentException($"does not support at platform {Application.platform}");
            }
        }
    }
}
