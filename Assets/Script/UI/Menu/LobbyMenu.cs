using System;
using System.Collections.Generic;
using Network;
using SocketIO;
using Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public struct RoomInfo
    {
        public string Id;
        public int PlayerCount;
        public string Name;
        public string Type;
    }
    public class LobbyMenu : MonoBehaviour
    {
        private SocketIOComponent _network;
        private readonly List<RoomInfo> _roomInfos = new List<RoomInfo>();
        [SerializeField] private Text serverName;
        [SerializeField] private RectTransform canvas;
        [SerializeField] private GameObject options;
        [SerializeField] private GameObject waitWindow;
        [SerializeField] private GameObject roomWindow;
        [SerializeField] private GameObject errorWindow;
        [SerializeField] private string sceneName;
        private void Start()
        {
            RoomOption.WaitingWindow = waitWindow;
            serverName.text = GameChoice.ServerName;
            _network = NetworkManager.GetInstance().GetComponent();
            _network.On("listRoom" , _getRoomInfo);
            _network.Emit("listRoom");
            _network.On("joinRoom",_joinRoomResult);
            _optionWidth = options.GetComponent<RectTransform>().rect.height;
            _network.On("startGame" , _startGame);
        }
        
        private float _optionWidth;
        private void _renderOptions()
        {
            canvas.offsetMin = new Vector2(canvas.offsetMin.x, -_optionWidth * _roomInfos.Count);
            canvas.ForceUpdateRectTransforms();
            var topY= canvas.rect.height / 2;
            for (var u = 0 ; u != _roomInfos.Count ; ++u)
            {
                var option = Instantiate(options, canvas.transform);
                var yPos = topY - _optionWidth / 2 - _optionWidth * u;

                var rectTransform = option.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0,yPos);
                option.GetComponent<RoomOption>().SetInfo(_roomInfos[u]);
            }
        }

        private static RoomInfo _parseRoomInfo(JSONObject jsonObject)
        {
            var roomInfo = new RoomInfo
            {
                Id = jsonObject["id"].str,
                PlayerCount = (int) jsonObject["playerCount"].n,
                Name = jsonObject["name"].str,
                Type = jsonObject["type"].str
            };
            return roomInfo;
        }
        private void _getRoomInfo(SocketIOEvent e)
        {
            _roomInfos.Clear();
            foreach (var jsonObject in e.data["rooms"].list)
            {
                _roomInfos.Add(_parseRoomInfo(jsonObject));
            }
            UnityMainThread.Worker.AddJob(_renderOptions);
        }
        private void _joinRoomResult(SocketIOEvent e)
        {
            UnityMainThread.Worker.AddJob(() =>
            {
                waitWindow.SetActive(false);
            });
            
            if (e.data["success"].b)
            {
                print("successfully joined the room");
                UnityMainThread.Worker.AddJob(() =>
                {
                    roomWindow.SetActive(true);
                    var window = roomWindow.GetComponent<RoomWindow>();
                    window.SetInfo( (int) e.data["maxPlayer"].n , e.data["roomName"].str );
                });
            }
            else
            {
                print($"failed joining the room with reason {e.data["msg"].str}");
                UnityMainThread.Worker.AddJob(() =>
                {
                    errorWindow.SetActive(true);
                    errorWindow.GetComponent<ErrorWindow>().SetMessage(e.data["msg"].str);
                });
            }
        }

        private void _startGame(SocketIOEvent e) =>
            UnityMainThread.Worker.AddJob(() => SceneManager.LoadScene(sceneName));

        private void OnDestroy()
        {
            _network.Off("listRoom" , _getRoomInfo);
            _network.Off("joinRoom",_joinRoomResult);
            _network.Off("startGame", _startGame);
        }
    }
}