using UnityEngine;
using UUID;
using Init;
namespace Map.Platforms
{
    public class TimePlatform : UuidObject , IPlatform
    {
        private float _speed = MapFactory.GlobalSpeed;
        [SerializeField] private float scale = 1.0f;
        [SerializeField] private float timeCost = 4.3f;
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            transform.Translate(Vector3.down * (Time.deltaTime * scale * _speed));
        }
        private void OnCollisionEnter2D(Collision2D collisionInfo)
        {
            if(!collisionInfo.gameObject.CompareTag("Player")) return;
            GameRound.Instance.StartCoroutine("_minusTime" , timeCost);
            GameManager.Instance.creator.PlatformConstructor(this.transform.position , Vector2.one*MapFactory.PlatformScale , PlatformTypes.Normal);
            Destroy(this.gameObject);
        }
    }
}