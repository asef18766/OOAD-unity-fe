using UnityEngine;
using UUID;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Map.Platforms
{
    public class SpikePlatform : UuidObject , IPlatform
    {
        private float _speed = MapFactory.GlobalSpeed;

        [SerializeField] private float scale = 1.0f;
        [SerializeField] private float waitDuration = 1.0f;
        [SerializeField] private float dmg = 5;
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            transform.Translate(Vector3.down * (Time.deltaTime * scale * _speed));
        }

        private IEnumerator _addStall(Guid id)
        {
            _waitingList.Add(id);
            yield return new WaitForSeconds(waitDuration);
            _waitingList.Remove(id);
        }
        private readonly List<Guid> _waitingList = new List<Guid>();

        private void OnCollisionStay2D(Collision2D collisionInfo)
        {
            if(!collisionInfo.gameObject.CompareTag("Player")) return;
            var target = collisionInfo.gameObject.GetComponent<Player>();
            var playerUuid = target.uuid;
            if(_waitingList.Exists( x => playerUuid == x )) return;

            StartCoroutine(_addStall(playerUuid));
            target.StartCoroutine("_hurt",dmg);
        }
    } 
}