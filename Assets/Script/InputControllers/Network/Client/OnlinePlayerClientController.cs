using System;
using Init.Methods;
using UnityEngine;
using InputControllers.Pc;
using InputControllers.Android;
using SocketIO;
using Network;

namespace InputControllers.Network.Client
{
    public class OnlinePlayerClientController : MonoBehaviour, IPlayerController
    {
        private IPlayerController _controller;
        private bool _pressed = false;
        private Vector2 _move = Vector2.zero;
        private SocketIOComponent _network;
        private readonly Vector2[] _dir = { Vector2.left, Vector2.zero, Vector2.right };

        public void Init()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _controller = gameObject.AddComponent<AppPlayerController>();
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    _controller = gameObject.AddComponent<PcPlayerController>();
                    break;
                default:
                    throw new ArgumentException("Unknown Argument");
            }
            _network = NetworkManager.GetInstance().GetComponent();
        }

        void Update()
        {
            int move;
            if (_controller.OnMove().x > 0) move = 1;
            else if (Math.Abs(_controller.OnMove().x) < float.Epsilon) move = 0;
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
    }
}