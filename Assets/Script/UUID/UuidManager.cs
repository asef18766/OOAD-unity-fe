using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Network;
using SocketIO;
using Utils;
using UnityEngine;
using Object = UnityEngine.Object;
using Map.Platforms;

namespace UUID
{
    public class UuidManager
    {
        [CanBeNull] private static UuidManager _instance;
        private readonly Dictionary<Guid, UuidObject> _data;
        private readonly SocketIOComponent _network;
        private const float _emitSpeed = 0.5f;
        private UuidManager()
        {
            _data = new Dictionary<Guid, UuidObject>();
            _network = NetworkManager.GetInstance().GetComponent();
            if (GameChoice.GameMode == GameMode.Server)
                CoroutineRunner.Runner.StartCoroutine(_sendMovement());
            else
                _network.On("initOver", _receiveInitOver);
        }

        public static UuidManager GetInstance()
        {
            return _instance ?? (_instance = new UuidManager());
        }

        public void Register(UuidObject obj)
        {
            if(_data.ContainsKey(obj.uuid))
                return;
            _data.Add(obj.uuid, obj);
        }

        public UuidObject Query(Guid uuid)
        {
            return _data.ContainsKey(uuid) ? _data[uuid] : null;
        }
        public void Remove(Guid uuid)
        {
            if (!_data.ContainsKey(uuid)) return;
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
        private int _playerCount = 0;
        private void _instantiateObject(JSONObject jsonObject)
        {
            var guid = Guid.Parse(jsonObject["uuid"].str);
            var position = Jsonify.JsontoVector(jsonObject["transform"]["position"]);
            var rotation = Jsonify.JsontoVector(jsonObject["transform"]["rotation"]);
            var prefab = jsonObject["prefab"].str;

            switch (prefab)
            {
                case "Player":
                    UnityMainThread.Worker.AddJob(() =>
                    {
                        _playerCount++;
                        var pref = PrefabManager.GetInstance().GetGameObject($"P{_playerCount}Sprite");
                        var obj = Object.Instantiate(pref, position, Quaternion.Euler(rotation));
                        obj.transform.localScale = Vector3.one * 0.4f;
                        obj.GetComponent<UuidObject>().ModifySelfId(guid);
                    });
                    break;
                default:
                    UnityMainThread.Worker.AddJob(() =>
                    {
                        var pref = PrefabManager.GetInstance().GetGameObject(prefab);
                        var obj = Object.Instantiate(pref, position, Quaternion.Euler(rotation));
                        obj.GetComponent<UuidObject>().ModifySelfId(guid);
                    });
                    break;
            }
        }

        private void _translateObject(JSONObject jsonObject)
        {
            var uuid = Guid.Parse(jsonObject["uuid"].str);
            if (!_data.ContainsKey(uuid))
            {
                Debug.Log($"{uuid} is not in uuid dictionary");
                return;
            }
            var obj = _data[uuid];
            obj.transform.position = Jsonify.JsontoVector(jsonObject["position"]);
            obj.transform.rotation = Quaternion.Euler(Jsonify.JsontoVector(jsonObject["rotation"]));
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
                    _translateObject(args);
                    break;
                case "Invoke":
                    throw new NotImplementedException();
            }
        }

        private void _receiveInitOver(SocketIOEvent e)
        {
            UnityMainThread.Worker.AddJob(() => _network.Emit("ready"));
        }

        private IEnumerator _sendMovement()
        {
            while (true)
            {
                yield return new WaitForSeconds(_emitSpeed);

                foreach (var obj in _data.Values)
                {
                    if (!(obj is IPlatform) && !(obj is Player))
                        continue;

                    var jsonObject = new JSONObject($"{{\"type\":\"Translate\"}}");
                    jsonObject["args"] = new JSONObject("{\"uuid\":\"a\"}");
                    jsonObject["args"]["position"] = Jsonify.VectortoJson(obj.transform.position);
                    jsonObject["args"]["rotation"] = Jsonify.VectortoJson(obj.transform.rotation.eulerAngles);
                    jsonObject["args"]["uuid"].str = obj.uuid.ToString();
                    _network.Emit("updateEntity", jsonObject);
                }
            }
        }
    }
}