using System;
using UnityEngine;

namespace InputControllers.Pc
{
    public class PcPlayerController : MonoBehaviour, IPlayerController
    {
        private PcKeyboardModel _model;

        private void Awake()
        {
            _model = gameObject.AddComponent<PcKeyboardModel>();
        }

        public bool OnClicked()
        {
            return _model.GetPressed();
        }

        public Vector2 OnMove()
        {
            return _model.GetMove();
        }
    }
}