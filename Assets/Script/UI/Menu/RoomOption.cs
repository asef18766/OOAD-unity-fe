using System;
using Network;
using SocketIO;
using ThreadUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu
{
    public class RoomOption : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Text roomId;
        [SerializeField] private Text roomName;
        [SerializeField] private Text mode;
        [SerializeField] private Text playerCount;
        public static GameObject WaitingWindow = null;
        private RoomInfo _roomInfo;

        private void Start()
        {
            button.onClick.AddListener(_joinRoom);
        }
        public void SetInfo(RoomInfo info)
        {
            _roomInfo = info;
            roomId.text = _roomInfo.Id;
            roomName.text = _roomInfo.Name;
            mode.text = _roomInfo.Type;
            playerCount.text = $"{_roomInfo.PlayerCount}/2";
        }

        private void _joinRoom()
        {
            print("try join room");
            WaitingWindow.SetActive(true);
            var jsonObject = new JSONObject($"{{\"roomID\":\"{_roomInfo.Id}\"}}");
            NetworkManager.GetInstance().GetComponent().Emit("joinRoom" , jsonObject);
        }
    }
}