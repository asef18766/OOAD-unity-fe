using UnityEngine;
using UUID;
using Network;

namespace Map
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlatformDestroyer : MonoBehaviour
    {
        private static readonly JSONObject UpdateEntityFormat = new JSONObject($"{{\"type\":\"Destroy\",\"args\":{{\"uuid\":\"a\"}}}}");
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Platform"))
            {
                var uuid = other.gameObject.GetComponent<UuidObject>().uuid;
                UuidManager.GetInstance().Remove(uuid);

                if(GameChoice.IsServer())
                {
                    var jsonObject = UpdateEntityFormat.Copy();
                    jsonObject["args"]["uuid"].str = uuid.ToString();
                    NetworkManager.GetInstance().GetComponent().Emit("updateEntity", jsonObject);
                }
                Destroy(other.gameObject);
            }
        }
    }
}