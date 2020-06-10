using System;
using System.Collections.Generic;
using UnityEngine;
using UUID;
using Random = UnityEngine.Random;

namespace Map.Platforms
{
    enum DirectionPlatformMode
    {
        LeftPlatform,
        RightPlatform,
        SwapPlatform
    }
    public class DirectionPlatform : UuidObject , IPlatform
    {
        //TODO: finish implementation
        private float _speed = MapFactory.GlobalSpeed;
        [SerializeField] private SpriteRenderer[] criticalSprite;
        [SerializeField] private Sprite rightArrow, swap , brick , add , subtract;
        [SerializeField] private float scale = 1.0f;
        [SerializeField] private float deltaSpeed = 0.3f;
        private DirectionPlatformMode _type;
        private bool _accelerate;
        private readonly Dictionary<DirectionPlatformMode, float> _spawnRate=new Dictionary<DirectionPlatformMode, float>()
        {
            {DirectionPlatformMode.LeftPlatform , 0.4f},
            {DirectionPlatformMode.RightPlatform , 0.4f},
            {DirectionPlatformMode.SwapPlatform , 0.2f}
        };
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }
        private DirectionPlatformMode ChoosePlatformType()
        {
            var number = Random.Range(0.0f, 1.0f);
            foreach (var item in _spawnRate)
            {
                number -= item.Value;
                if (number <= 0)
                    return item.Key;
            }

            throw new ArgumentException($"can not found match item with probability of {number}");
        }
        private void Start()
        {
            switch (ChoosePlatformType())
            {
                case DirectionPlatformMode.LeftPlatform:
                    _type = DirectionPlatformMode.LeftPlatform;
                    _accelerate = Random.Range(0, 2) == 0;
                    criticalSprite[0].sprite = (_accelerate)? add:subtract;
                    criticalSprite[1].sprite = rightArrow;
                    criticalSprite[1].flipX = (_accelerate)? add:subtract;
                    criticalSprite[2].sprite = brick;
                    break;
                case DirectionPlatformMode.RightPlatform:
                    _type = DirectionPlatformMode.RightPlatform;
                    _accelerate = Random.Range(0, 2) == 0;
                    criticalSprite[0].sprite = (_accelerate)? add:subtract;
                    criticalSprite[1].sprite = rightArrow;
                    criticalSprite[2].sprite = (_accelerate)? add:subtract;
                    break;
                case DirectionPlatformMode.SwapPlatform:
                    _type = DirectionPlatformMode.SwapPlatform;
                    criticalSprite[0].sprite = brick;
                    criticalSprite[1].sprite = swap;
                    criticalSprite[2].sprite = brick;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void Update()
        {
            transform.Translate(Vector3.down * (Time.deltaTime * scale * _speed));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!other.gameObject.CompareTag("Player")) return;

            var player = other.gameObject.GetComponent<Player>();
            switch (_type)
            {
                case DirectionPlatformMode.LeftPlatform:
                    player.leftAccelerate = (_accelerate) ? 1 + deltaSpeed : 1 - deltaSpeed;
                    break;
                case DirectionPlatformMode.RightPlatform:
                    player.rightAccelerate = (_accelerate) ? 1 + deltaSpeed : 1 - deltaSpeed;
                    break;
                case DirectionPlatformMode.SwapPlatform:
                    player.leftAccelerate = -1;
                    player.rightAccelerate = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            if(!other.gameObject.CompareTag("Player")) return;
            other.gameObject.GetComponent<Player>().StartCoroutine(nameof(Player.ResetSpeed));
        }
    }
}