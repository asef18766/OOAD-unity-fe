using System.Dynamic;
using UnityEngine;

namespace UUID
{
    public class UuidObject : MonoBehaviour
    {
        public System.Guid uuid;
        public UuidObject()
        {
            uuid = System.Guid.NewGuid();
            UuidManager.GetInstance().Register(this);
        }
        public UuidObject(string remoteId)
        {
            uuid = System.Guid.Parse(remoteId);
        }
        public void ModifySelfId(System.Guid id)
        {
            UuidManager.GetInstance().Remove(uuid);
            uuid = id;
            UuidManager.GetInstance().Register(this);
        }
    }
}