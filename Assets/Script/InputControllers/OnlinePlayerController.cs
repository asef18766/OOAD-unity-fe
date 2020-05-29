using System;
using UnityEngine;
using InputControllers.Pc;
using InputControllers.Android;
using SocketIO;
using Network;

namespace InputControllers
{
    public class OnlinePlayerController : MonoBehaviour, IPlayerController
    {
        private IPlayerController _controller;
        private bool _pressed = false;
        private Vector2 _move = Vector2.zero;
        private SocketIOComponent _network;
        private readonly Vector2[] _dir = { Vector2.left, Vector2.zero, Vector2.right };

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

            /* for testing
            _network.url = $"ws://127.0.0.1/socket.io/?EIO=4&transport=websocket";
            _network.Connect();*/
        }

        void Update()
        {
            int move;
            if (_controller.OnMove().x > 0) move = 1;
            else if (_controller.OnMove().x == 0) move = 0;
            else move = -1;

            var jsonObject = new JSONObject($"{{\"move\":{move},\"function\":{_controller.OnClicked()}}}");
            _network.Emit("operation", jsonObject);
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
            _pressed = e.data["function"].b;
            _move = _dir[(int)e.data["move"].n + 1];
        }
    }
}