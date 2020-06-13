using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Network;
using SocketIO;
using Utils;
using Network;
using Map;
using Map.Platforms;
using UnityEngine;
using Object = UnityEngine.Object;
using Map.Platforms;

namespace UUID
{
    public class UuidManager
    {
        [CanBeNull] private static UUID.UuidManager _instance = null;
        private readonly Dictionary<System.Guid, UuidObject> _data;
        private SocketIOComponent _network;
        private const float _emitSpeed = 0.5f;
        private static readonly JSONObject UpdateEntityFormat = new JSONObject($"{{\"type\":\"Translate\"}}");
        private static readonly JSONObject ArgsFormat = new JSONObject("{\"uuid\":\"a\"}");
        private UuidManager()
        {
            _data = new Dictionary<Guid, UuidObject>();
            CoroutineRunner.Runner.StartCoroutine(_sendMovement());
            _network = NetworkManager.GetInstance().GetComponent();
        }

        public static UuidManager GetInstance()
        {
            return _instance ?? (_instance = new UuidManager());
        }

        public void Register(UuidObject obj)
        {
            _data.Add(obj.uuid, obj);
        }

        public UuidObject Query(System.Guid uuid)
        {
            return _data.ContainsKey(uuid) ? _data[uuid] : null;
        }
        public void Remove(System.Guid uuid)
        {
            if (_data.ContainsKey(uuid))
                _data.Remove(uuid);
        }
        public void HookNetworking()
        {
            NetworkManager.GetInstance().GetComponent().On("updateEntity", _onUpdateEntity);
        }
        public void UnHookNetworking()
        {
            NetworkManager.GetInstance().GetComponent().Off("updateEntity", _onUpdateEntity);
        }

        /*
         * format:
         * {
         *     "uuid":String ,
         *     "parent":String ,
         *     "transform":
         *     {
         *         "position":[0,0,0] ,
         *         "rotation":[0,0,0]
         *     },
         *     "prefab":String
         * }
        */
        private void _instantiateObject(JSONObject jsonObject)
        {
            var guid = Guid.Parse(jsonObject["uuid"].str);
            var position = Jsonify.JsontoVector(jsonObject["transform"]["position"]);
            var rotation = Jsonify.JsontoVector(jsonObject["transform"]["rotation"]);
            var prefab = jsonObject["prefab"].str;
            
            UnityMainThread.Worker.AddJob(() =>
            {
                var pref = PrefabManager.GetInstance().GetGameObject(prefab);
                Object.Instantiate(pref , position , Quaternion.Euler(rotation));
            });
            throw new NotImplementedException();
        }

        private void _onUpdateEntity(SocketIOEvent e)
        {
            Debug.Log($"receive packet updateEntity:{e.data}");
            
            var cmd = e.data["type"].str;
            var args = e.data["args"];
            switch (cmd)
            {
                case "Instantiate":
                    _instantiateObject(args);
                    break;
                case "Translate":
                    throw new NotImplementedException();
                    break;
                case "Invoke":
                    throw new NotImplementedException();
                    break;
            }
        }

        private IEnumerator _sendMovement()
        {
            while (true)
            {
                yield return new WaitForSeconds(_emitSpeed);

                foreach (var obj in _data.Values)
                {
                    var jsonObject = UpdateEntityFormat.Copy();
                    jsonObject["args"] = ArgsFormat.Copy();
                    jsonObject["args"]["position"] = Jsonify.VectortoJson(obj.transform.position);
                    jsonObject["args"]["rotation"] = Jsonify.VectortoJson(obj.transform.rotation.eulerAngles);
                    jsonObject["args"]["uuid"].str = obj.uuid.ToString();
                    _network.Emit("updateEntity", jsonObject);
                }
            }
            yield break;
        }

        public void PauseGame(bool pause)
        {
            MapFactory.GetInstance().IsPause = pause;

            foreach (var obj in _data.Values)
            {
                obj.gameObject.SetActive(!pause);
            }
        }
    }
}