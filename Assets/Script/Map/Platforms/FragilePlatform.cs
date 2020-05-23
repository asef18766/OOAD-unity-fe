using UnityEngine;
using UUID;
namespace Map.Platforms
{
    public class FragilePlatform : UuidObject , IPlatform
    {
        private float _speed = 1;

        [SerializeField] private float scale = 1.0f;
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            transform.Translate(Vector3.down * (Time.deltaTime * scale * _speed));
        }
    } 
}