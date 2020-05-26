using System;
using Event;
using UnityEngine;
using UUID;

namespace InputControllers.Android
{
    public class AppPadView : UuidObject
    {
        [SerializeField] private Sprite attack;
        [SerializeField] private Sprite jump;

        private PlayerState _state;
        private void Swap(string e , JSONObject obj)
        {
            switch (_state)
            {
                case PlayerState.Jump:
                    _state = PlayerState.Attack;
                    break;
                case PlayerState.Attack:
                    _state = PlayerState.Jump;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private SpriteRenderer _spriteRenderer;

        public AppPadView(PlayerState state)
        {
            _state = state;
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            EventManager.GetInstance().RegisterEvent("Swap",Swap);
        }

        private void Update()
        {
            switch (_state)
            {
                case PlayerState.Attack:
                    _spriteRenderer.sprite = attack;
                    break;
                case PlayerState.Jump:
                    _spriteRenderer.sprite = jump;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}