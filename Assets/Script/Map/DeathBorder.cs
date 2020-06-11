using System;
using UnityEngine;

namespace Map
{
    public class DeathBorder : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.CompareTag("Player")) return;
            other.gameObject.GetComponent<Player>().StartCoroutine("_hurt", 100);
        }
    }
}