using UnityEngine;

namespace InputControllers.Android
{
    public class AppPlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField]private AppPadModel moveLStick=null;
        [SerializeField] private AppPadModel moveRStick = null;
        [SerializeField]private AppPadModel attackStick=null;
        private void Start()
        {
            Input.simulateMouseWithTouches = true;
        }

        public Vector2 OnMove()
        {
            /*
            return  moveStick.GetMove();
            */
            return Vector2.zero;
        }
    
        public bool OnClicked()
        {
            return attackStick.GetPressed();
        }
    }
}
