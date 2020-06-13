using System;
using System.Collections.Generic;
using Network;
using SocketIO;
using UnityEngine;
using Utils;

namespace Event
{
    public class EventManager
    {
        private static Dictionary<string, List<Action<string,JSONObject>>> _events = null;
        private static EventManager _instance=null;
        private EventManager()
        {
            _events = new Dictionary<string, List<Action<string,JSONObject>>>();
        }

        public void RegisterEvent(string eventName, Action<string,JSONObject> handler)
        {
            if (!_events.ContainsKey(eventName))
            {
                Debug.Log($"create list with eventName {eventName}");
                _events.Add(eventName , new List<Action<string,JSONObject>>());
            }
            _events[eventName].Add(handler);
            Debug.Log($"register successfully with event name {eventName}");
        }
        public void UnRegisterEvent(string eventName, Action<string,JSONObject> handler)
        {
            if (_events.ContainsKey(eventName))
            {
                _events[eventName].Remove(handler);
            }
        }
        public void InvokeEvent(string eventName,JSONObject args)
        {
            if (!_events.ContainsKey(eventName)) 
                return;
            
            foreach (var action in _events[eventName])
            {
                action(eventName,args);
            }
        }

        private void _onInvokeEvent(SocketIOEvent ev)
        {
            var d = ev.data;
            var eventName = d["name"].str;
            var args = d["args"];
            if (!_events.ContainsKey(eventName))
            {
                Debug.Log($"Haven't register event {eventName}");
                return;
            }

            foreach (var cAction in _events[eventName])
            {
                UnityMainThread.Worker.AddJob(() =>
                {
                        cAction(eventName , args);
                });
            }
        }
        public void HookNetworking()
        {
            NetworkManager.GetInstance().GetComponent().On("invokeEvent" , _onInvokeEvent);
        }
        public static EventManager GetInstance()
        {
            return _instance ?? (_instance = new EventManager());
        }
        public void Clean()
        {
            _events.Clear();
        }
    }
}