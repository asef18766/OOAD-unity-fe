using System;
using System.Collections.Generic;

namespace ThreadUtils
{
    public class UnityMainThread : UnityEngine.MonoBehaviour
    {
        internal  static UnityMainThread Worker;
        private Queue<Action> _jobs = new Queue<Action>();

        private void Awake() {
            Worker = this;
            DontDestroyOnLoad(this);
        }

        private void Update() {
            while (_jobs.Count > 0) 
                _jobs.Dequeue().Invoke();
        }

        internal void AddJob(Action newJob) {
            _jobs.Enqueue(newJob);
        }
    }
}