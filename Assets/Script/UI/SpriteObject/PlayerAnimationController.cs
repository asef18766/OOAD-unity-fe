using Event;
using UnityEngine;
using UUID;

namespace UI.SpriteObject
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : UuidObject
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer , _iceSpriteRenderer;
        private bool _idle = true;
        private Vector2 _facing = Vector2.left;
        [SerializeField] private GameObject iceRef;
        private static readonly int Moving = Animator.StringToHash("moving");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _iceSpriteRenderer = iceRef.GetComponent<SpriteRenderer>();
            EventManager.GetInstance().RegisterEvent("swap" ,Swap);
        }
        
        private void _playWalk() => _animator.SetBool(Moving , true);
        private void _playerIdle() => _animator.SetBool(Moving , false);

        private void Swap(string ev, JSONObject jsonObject)
        {
            _spriteRenderer.flipY = !_spriteRenderer.flipY;
            _iceSpriteRenderer.flipY = !_iceSpriteRenderer.flipY;
        }
        public void PlayerMove(Vector2 vec)
        {
            if (vec == Vector2.zero)
            {
                print($"{this.gameObject.name} is idling");
                if (!_idle)
                {
                    print("set idle");
                    _playerIdle();
                    _idle = true;
                }
                
            }
            else
            {
                if (_facing.x * vec.x < 0)
                {
                    _spriteRenderer.flipX = !_spriteRenderer.flipX;
                    _facing = vec;
                }

                if (_idle)
                {
                    _playWalk();
                    _idle = false;
                }
            }
        }
        public void PlayerFreeze(Player _) => iceRef.SetActive(true);
        public void PlayerUnFreeze(Player _) => iceRef.SetActive(false);
    }
}