using System;
using System.Collections.Generic;
using System.Linq;

namespace Event
{
    public class EventManager
    {
        private static Dictionary<string, List<Action<string>>> _events = null;
        private static EventManager _instance=null;
        private EventManager()
        {
            _events = new Dictionary<string, List<Action<string>>>();
        }

        public void RegisterEvent(string eventName, Action<string> handler)
        {
            if (!_events.ContainsKey(eventName))
            {
                _events.Add(eventName , new List<Action<string>>());
            }
            _events[eventName].Append(handler);
        }
        public void UnRegisterEvent(string eventName, Action<string> handler)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName].Remove(handler);
            }
        }
        public void InvokeEvent(string eventName)
        {
            if (!_events.ContainsKey(eventName)) 
                return;
            
            foreach (var action in _events[eventName])
            {
                action(eventName);
            }
        }

        public static EventManager GetInstance()
        {
            return _instance ?? (_instance = new EventManager());
        }
    }
}