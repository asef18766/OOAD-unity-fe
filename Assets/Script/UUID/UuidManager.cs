using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UUID
{
    public class UuidManager
    {
        [CanBeNull] private static UUID.UuidManager _instance = null;
        private readonly Dictionary<System.Guid, UuidObject> _data; 
        private UuidManager()
        {
            _data = new Dictionary<Guid, UuidObject>();
        }

        public static UuidManager GetInstance()
        {
            return _instance ?? (_instance = new UuidManager());
        }

        public void Register(UuidObject obj)
        {
            _data.Add(obj.uuid ,obj);
        }

        public UuidObject Query(System.Guid uuid)
        {
            return _data.ContainsKey(uuid) ? _data[uuid] : null;
        }
        public void Remove(System.Guid uuid)
        {
            if(_data.ContainsKey(uuid))
                _data.Remove(uuid);
        }
    }
}