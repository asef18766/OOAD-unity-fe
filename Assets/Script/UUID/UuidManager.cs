using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Network;
using SocketIO;
using ThreadUtils;
using Network;

namespace UUID
{
    public class UuidManager
    {
        [CanBeNull] private static UUID.UuidManager _instance = null;
        private readonly Dictionary<System.Guid, UuidObject> _data; 
        private SocketIOComponent _network;
        private UuidManager()
        {
            _data = new Dictionary<Guid, UuidObject>();
            //CoroutineRunner.Runner.StartCoroutine(_sendMovement());
            _network = NetworkManager.GetInstance().GetComponent();
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
        public void HookNetworking()
        {
            NetworkManager.GetInstance().GetComponent().On("updateEntity" , _onUpdateEntity);
        }
        public void UnHookNetworking()
        {
            NetworkManager.GetInstance().GetComponent().Off("updateEntity" , _onUpdateEntity);
        }
        
        /*
         * format:
         * {
         *     "uuid":String ,
         *     "parent":String ,
         *     "transform":
         *     {
         *         "position":[0,0,0] ,
         *         "rotation":[0,0,0] ,
         *         "scale":[0,0,0]
         *     },
         *     "prefab":String
         * }
        */
        private void _instantiateObject(JSONObject jsonObject)
        {
            var guid = Guid.Parse(jsonObject["uuid"].str);
            var parent = jsonObject["parent"].str;
            throw new NotImplementedException();
        }
        
        private void _onUpdateEntity(SocketIOEvent e)
        {
            var cmd = e.data["type"].str;
            var args = e.data["args"];
            
            throw new NotImplementedException();
        }

        private void _sendMovement()
        {
            _network.Emit("operation");
        }
    }
}