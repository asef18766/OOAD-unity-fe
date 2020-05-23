using System;
using System.Collections;
using System.Collections.Generic;
using Event;
using Init;
using Map.Platforms;
using ThreadUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class MapFactory
    {
        private static MapFactory _instance = null;
        private readonly Dictionary<PlatformTypes, float> _spawnRate = null;
        private List<IPlatform> _platforms = new List<IPlatform>();
        private const float SpawnSpeed = 2.0f;

        private const float YLocation = 0;
        private const float XMinLocation = 0;
        private const float XMaxLocation = 0;

        public static float PlatformScale = 2.0f;
        
        private MapFactory()
        {
            var eventManager = EventManager.GetInstance();
            eventManager.RegisterEvent("CreatePlatform" , CreatePlatform);
            _spawnRate = new Dictionary<PlatformTypes, float>
            {
                {PlatformTypes.Direction, 0.1f},
                {PlatformTypes.Fragile, 0.1f},
                {PlatformTypes.Freeze, 0.1f},
                {PlatformTypes.Spike, 0.1f},
                {PlatformTypes.Time, 0.1f},
                {PlatformTypes.Normal, 0.5f}
            };
            _instance = this;
            CoroutineRunner.Runner.StartCoroutine(GeneratePlatform());
        }
        public static MapFactory GetInstance()
        {
            return _instance ?? (_instance = new MapFactory());
        }

        private PlatformTypes ChoosePlatformType()
        {
            var number = Random.Range(0.0f, 1.0f);
            foreach (var item in _spawnRate)
            {
                number -= item.Value;
                if (number <= 0)
                    return item.Key;
            }

            throw new ArgumentException($"can not found match item with probability of {number}");
        }
        private IEnumerator GeneratePlatform()
        {
            yield return new WaitForSeconds(SpawnSpeed);
            var platformType= ChoosePlatformType();
            var xPos = Random.Range(XMinLocation, XMaxLocation);
            CreatePlatform(platformType , new Vector2(xPos , YLocation), PlatformScale);
        }
        
        /*
         * obj form:
         * {
         *     "type":"NormalPlatform",
         *     "location":[
         *         4 , 8 , 6
         *     ],
         *     "scale":87
         * }
         */
        private static void CreatePlatform(PlatformTypes type , Vector2 pos , float scale)
        {
            var plat = GameManager.Instance.creator.PlatformConstructor(pos, new Vector2(scale, scale), type);
            if (plat == null)
            {
                Debug.Log("GG");
            }
        }
        private void CreatePlatform(string name , JSONObject obj)
        {
            var location = obj["location"].list;
            var loc = new Vector3(location[0].f , location[1].f , location[2].f);
            var scale = obj["scale"].f;
            
            if(!Enum.TryParse(obj["type"].str , out PlatformTypes platformType))
                throw new ArgumentException($"invalid platform type {obj["type"].str}");
            
            CreatePlatform(platformType , loc , scale);
        }
    }
}