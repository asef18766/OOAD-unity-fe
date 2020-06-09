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
                    criticalSprite[0].sprite = brick;
                    criticalSprite[1].sprite = rightArrow;
                    criticalSprite[1].flipX = true;
                    criticalSprite[2].sprite = brick;
                    _accelerate = Random.Range(0, 2) == 0;
                    break;
                case DirectionPlatformMode.RightPlatform:
                    _type = DirectionPlatformMode.RightPlatform;
                    criticalSprite[0].sprite = brick;
                    criticalSprite[1].sprite = rightArrow;
                    criticalSprite[2].sprite = brick;
                    _accelerate = Random.Range(0, 2) == 0;
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
            throw new NotImplementedException();
        }

        private void OnCollisionExit(Collision other)
        {
            throw new NotImplementedException();
        }
    }
}