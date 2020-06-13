using System.Dynamic;
using UnityEngine;

namespace UUID
{
    public class UuidObject : MonoBehaviour
    {
        public System.Guid uuid;

        private void Start()
        {
            if(GameChoice.GameMode == GameMode.Online) return;
            uuid = System.Guid.NewGuid();
            UuidManager.GetInstance().Register(this);
        }
        public void ModifySelfId(System.Guid id)
        {
            UuidManager.GetInstance().Remove(uuid);
            uuid = id;
            UuidManager.GetInstance().Register(this);
        }
    }
}