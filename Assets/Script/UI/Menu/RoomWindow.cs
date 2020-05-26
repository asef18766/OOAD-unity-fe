using System;
using Network;
using SocketIO;
using ThreadUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    
    public class RoomWindow : MonoBehaviour
    {
        [SerializeField] private Text roomName;
        [SerializeField] private Image p1, p2;
        [SerializeField] private Button exitButton;
        [SerializeField] private Text playerCount;
        [SerializeField] private Sprite joined, notJoined;
        private int _maxPlayerCount = 0;
        private int _currentPlayerCount = 0;

        public void SetInfo(int maxCount , string rName)
        {
            _maxPlayerCount = maxCount;
            roomName.text = rName;
            _currentPlayerCount = 1;
            playerCount.text = $"{_currentPlayerCount}/{_maxPlayerCount}";
        }

        private void _updateCount(SocketIOEvent e)
        {
            UnityMainThread.Worker.AddJob(() =>
            { 
                _currentPlayerCount = (int) e.data["count"].n;
                p1.sprite = _currentPlayerCount >= 1 ? joined : notJoined;
                p2.sprite = _currentPlayerCount >= 2 ? joined : notJoined;
                playerCount.text = $"{_currentPlayerCount}/{_maxPlayerCount}";
            });
        }

        private void Start()
        {
            exitButton.onClick.AddListener(() =>
            {
                NetworkManager.GetInstance().GetComponent().Emit("exitRoom");
                this.gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            NetworkManager.GetInstance().GetComponent().On("playerCount" , _updateCount);    
        }

        private void OnDisable()
        {
            NetworkManager.GetInstance().GetComponent().Off("playerCount" , _updateCount);
            _currentPlayerCount = 0;
            _maxPlayerCount = 0;
        }
    }
}