//#define BUILD_SERVER
using System;
using System.Collections;
using Event;
using Init.Methods;
using InputControllers.Pc;
using Map;
using Map.Platforms;
using Network;
using SocketIO;
using UI.SpriteObject;
using Utils;
using UnityEngine;
using UUID;

namespace Init
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public IObjectConstructor creator;

        [SerializeField] private Transform canvas;

        #region offline_PC_implementation

        private void _setPlayerSprite(GameObject sprite , Player player)
        {
            var playerAnimationController = Instantiate(sprite, player.transform).GetComponent<PlayerAnimationController>();
            player.moveCallBack.Add(playerAnimationController.PlayerMove);
            player.freezeCallBack.Add(new Tuple<Action<Player>, Action<Player>>(playerAnimationController.PlayerFreeze , playerAnimationController.PlayerUnFreeze));
        }
        
        private void _buildPcOffline()
        {
            #region map_creation
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
                creator.PlatformConstructor(location, Vector2.one * 2, PlatformTypes.Normal);
            #endregion
            #region player_creation
            const int pScale = 4;
            var p1 = creator.PlayerConstructor(new Vector2(-2.78f, 2.48f),Vector2.one * pScale, PlayerState.Jump);
            p1.name = "p1";
            var p2 = creator.PlayerConstructor(new Vector2(3.904f, -3.963f), Vector2.one * pScale, PlayerState.Attack);
            p2.GetComponent<PcKeyboardModel>().walk = new[]
            {
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.LeftArrow,
                KeyCode.RightArrow
            };
            p2.GetComponent<PcKeyboardModel>().clicked = KeyCode.KeypadEnter;
            p2.name = "p2";
            var prefabManager = PrefabManager.GetInstance();
            if(prefabManager == null)
                print("null prefab manager");
            _setPlayerSprite(prefabManager.GetGameObject("P1Sprite") , p1);
            _setPlayerSprite(prefabManager.GetGameObject("P2Sprite") , p2);
            #endregion

            var round = new GameObject("GameRound");
            round.AddComponent<GameRound>();

            var ui = PrefabManager.GetInstance().GetGameObject("UIController");
            Instantiate(ui, canvas);
        }

        #endregion
        #region online_server_implementation
        private void _serverCreated(SocketIOEvent e)
        {
            UnityMainThread.Worker.AddJob(() =>
            {
                StopCoroutine(_gameKiller);
            });
            print($"receive raw {e}");
            if (e.data["success"].b)
            {
                UnityMainThread.Worker.AddJob(() =>
                {
                    const int pScale = 4;
                    var p1 = creator.PlayerConstructor(new Vector2(-2.78f, 2.48f), Vector2.one * pScale, PlayerState.Jump);
                    var p2 = creator.PlayerConstructor(new Vector2(3.904f, -3.963f), Vector2.one * pScale, PlayerState.Attack);
                    p1.name = "p1";
                    p2.name = "p2";

                    var jsonObject = new JSONObject();
                    jsonObject.AddField("ids", new JSONObject(JSONObject.Type.ARRAY));
                    jsonObject["ids"].Add(p1.uuid.ToString());
                    jsonObject["ids"].Add(p2.uuid.ToString());
                    NetworkManager.GetInstance().GetComponent().Emit("setPlayerID", jsonObject);

                    #region map_creation
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
                        creator.PlatformConstructor(location, Vector2.one * 2, PlatformTypes.Normal);
                    #endregion
                });
            }
            else
                throw new ApplicationException("return failed on creating server");
        }

        private static IEnumerator _waitServer()
        {
            yield return new WaitForSeconds(5);
            Application.Quit(-1);
            print("quit game");
        }

        private IEnumerator _gameKiller;
        private void _buildOnlineServer()
        {
            UnityMainThread.Spawn();

            var manager = NetworkManager.GetInstance();
            var secret = "14508888";

            var network = manager.GetComponent();
            network.url = "ws://127.0.0.1/socket.io/?EIO=4&transport=websocket";

            var i = 0;
            network.On("open", (e) =>
           {
               if (i == 0)
                   network.Emit("serverCreated", JSONObject.Create($"{{\"token\":\"{secret}\"}}"));
               i++;
           });
            network.On("serverCreated", _serverCreated);
            network.Connect();
            _gameKiller = _waitServer();
            StartCoroutine(_gameKiller);
        }
        #endregion
        #region online_client_implementation
        private void _buildOnlineClient()
        {
            UuidManager.GetInstance().HookNetworking();
            creator.PlayerConstructor(Vector3.zero, Vector2.zero, PlayerState.Jump);
        }
        #endregion
        private void Start()
        {
            if (PrefabManager.GetInstance() == null)
                throw new ApplicationException("can not init prefab manager");

            Instance = this;
#if BUILD_SERVER
            GameChoice.Gamemode = GameMode.Server;
#endif
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    break;
                case RuntimePlatform.LinuxPlayer:
                    break;
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    if (GameChoice.GameMode == GameMode.Offline)
                    {
                        print("i am pc offline");
                        creator = new PcOfflineConstructor();
                        _buildPcOffline();
                    }
                    else if(GameChoice.GameMode == GameMode.Online)
                    {
                        creator = new OnlineClientConstructor();
                        _buildPcOffline();
                    }
                    else if (GameChoice.GameMode == GameMode.Server)
                    {
                        creator = new ServerConstructor();
                        _buildOnlineServer();
                    }
                    break;
                default:
                    throw new ArgumentException($"does not support at platform {Application.platform}");
            }
        }
    }
}
