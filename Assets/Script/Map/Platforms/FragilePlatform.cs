using Init;
using UnityEngine;
using UUID;
namespace Map.Platforms
{
    public class FragilePlatform : UuidObject , IPlatform
    {
        //TODO: finish implementation
        private float _speed = MapFactory.GlobalSpeed;

        [SerializeField] private float scale = 1.0f;
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            transform.Translate(Vector3.down * (Time.deltaTime * scale * _speed));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.collider.CompareTag("Player")) return;
            GameManager.Instance.creator.PlatformConstructor(transform.position , Vector2.one*MapFactory.PlatformScale , PlatformTypes.Normal);
            Destroy(gameObject);
        }
    } 
}