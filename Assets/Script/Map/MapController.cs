using System.Collections.Generic;
using Event;
using UnityEngine;

namespace Map
{
    public class MapFactory
    {
        private static MapFactory _instance = null;
        private Dictionary<string, float> _spawnRate = null;
        private PrefabManager _prefabManager = null;
        private MapFactory()
        {
            var eventManager = EventManager.GetInstance();
            eventManager.RegisterEvent("CreatePlatform" , CreatePlatform);
            _spawnRate = new Dictionary<string, float>
            {
                {"DirectionPlatform", 0.1f},
                {"FragilePlatform", 0.1f},
                {"FreezePlatform", 0.1f},
                {"SpikePlatform", 0.1f},
                {"TimePlatform", 0.1f},
                {"TimePlatform", 0.1f},
                {"NormalPlatform", 0.5f}
            };

            _prefabManager = PrefabManager.GetInstance();
        }
        public static MapFactory GetInstance()
        {
            return _instance ?? (_instance = new MapFactory());
        }
        
        /*
         * obj form:
         * {
         *     "type":"NormalPLatform",
         *     "location":[
         *         4 , 8 , 6
         *     ],
         *     "scale":87,
         *     "fallRate":87
         * }
         */
        private void CreatePlatform(string name , JSONObject obj)
        {
            var pref = _prefabManager.GetGameObject(obj["type"].str);
            var gameObject=Object.Instantiate(pref);
            var locations = obj["location"].list;
            gameObject.transform.position = new Vector3(locations[0].f , locations[1].f , locations[2].f);
        }
    }
}