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
        IEnumerator _addStall(Guid id)
        {
            _waitingList.Add(id);
            yield return new WaitForSeconds(waitDuration);
            _waitingList.Remove(id);
        }
        private List<Guid> _waitingList = new List<Guid>();
        void OnCollisionStay2D(Collision2D collisionInfo)
        {
            if(!collisionInfo.gameObject.CompareTag("Player")) return;
            var target = collisionInfo.gameObject.GetComponent<Player>();
            var playerUUID = target.uuid;
            if(_waitingList.Exists( x => playerUUID == x )) return;

            StartCoroutine(_addStall(playerUUID));
            target.StartCoroutine("_hurt",dmg);
        }
    } 
}