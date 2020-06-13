using System;
using System.Dynamic;
using UnityEngine;

namespace UUID
{
    public class UuidObject : MonoBehaviour
    {
        public Guid uuid;

        private void Start()
        {
            if (GameChoice.GameMode == GameMode.Online)
                return;
            print("i am not online mode");
            UuidManager.GetInstance().Register(this);
        }
        public void ModifySelfId(Guid id)
        {
            UuidManager.GetInstance().Remove(uuid);
            uuid = id;
            UuidManager.GetInstance().Register(this);
        }
    }
}