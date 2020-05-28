using System;
using UnityEngine;
using InputControllers.Pc;
using InputControllers.Android;
using SocketIO;

namespace InputControllers
{
    public class OnlinePlayerController : MonoBehaviour, IPlayerController
    {
        private IPlayerController _controller;
        private bool _online = false;
        private bool _pressed = false;
        private Vector2 _move = Vector2.zero;
        private SocketIOComponent _network;
        private const Vector2[] _dir = new Vector2[3] { Vecto2.left, Vector2.zero, Vector2.right };

        public void Init(String input)
        {
            switch (input)
            {
                case "pc":
                    _controller = gameObject.AddComponent<PcPlayerController>();
                    break;
                case "android":
                    _controller = gameObject.AddComponent<AppPlayerController>();
                    break;
                default:
                    Debug.Log("unknown input");
                    break;
            }

            _network = NetworkManager.GetInstance().GetComponent();
            _network.On("operation", _getOperation);
        }

        private void Awake()
        {
        }

        void Update()
        {
            if (_online)
            {
                int move;
                if (_controller.OnMove().x > 0) move = 1;
                else if (_controller.OnMove().x == 0) move = 0;
                else move = -1;

                var jsonObject = new JSONObject($"{{\"move\":\"{move}\",\"function\":\"{_controller.OnClicked()}\"}}");
                _network.Emit("operation", jsonObject);
            }
            else
            {
                _pressed = _controller.OnClicked();
                _move = _controller.OnMove();
            }
        }

        public bool OnClicked()
        {
            return _pressed;
        }

        public Vector2 OnMove()
        {
            return _move;
        }

        private void _getOperation(SocketIOEvent e)
        {
            _pressed = e.data["function"];
            _move = _dir[e.data["move"] + 1];
        }
    }
}