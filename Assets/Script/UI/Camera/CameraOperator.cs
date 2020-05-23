using Event;
using UnityEngine;

namespace UI.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraOperator : MonoBehaviour
    {
        private void Awake()
        {
            EventManager.GetInstance().RegisterEvent("swap" , Swap);
        }

        private void Swap(string eventName , JSONObject obj)
        {
            ReverseCamera();
        }
        public void ReverseCamera()
        {
            Transform transform1;
            (transform1 = this.transform).Rotate(0,180,180);
            transform1.position = -transform1.position;
        }
    }
}