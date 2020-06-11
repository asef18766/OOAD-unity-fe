using UnityEngine;

namespace Map
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlatformDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}