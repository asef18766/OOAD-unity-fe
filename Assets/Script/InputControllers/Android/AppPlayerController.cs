using UnityEngine;

namespace InputControllers.Android
{
    public class AppPlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField]private AppPadModel moveStick=null;
        [SerializeField]private AppPadModel attackStick=null;
        private void Start()
        {
            Input.simulateMouseWithTouches = true;
        }

        public Vector2 OnMove()
        {
            return  moveStick.GetMove();
        }
    
        public bool OnClicked()
        {
            return attackStick.GetPressed();
        }
    }
}
