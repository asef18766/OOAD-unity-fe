using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class UnityMainThread : UnityEngine.MonoBehaviour
    {
        internal static UnityMainThread Worker;
        private Queue<Action> _jobs = new Queue<Action>();

        private void Awake() {
            if(Worker != null) return;
            
            Worker = this;
            DontDestroyOnLoad(this);
        }

        public static void Spawn() => DontDestroyOnLoad(new GameObject("mainThread").AddComponent<UnityMainThread>().gameObject);
        private void Update() {
            while (_jobs.Count > 0) 
                _jobs.Dequeue().Invoke();
        }

        internal void AddJob(Action newJob) {
            _jobs.Enqueue(newJob);
        }
    }
}