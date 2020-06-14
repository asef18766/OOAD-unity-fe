using System;
using UnityEngine;

namespace InputControllers.Android
{
    public class AppPlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField]private AppPadModel leftMoveButton;
        [SerializeField]private AppPadModel rightMoveButton;
        [SerializeField]private AppPadModel functionButton;
        private void Start()
        {
            Input.simulateMouseWithTouches = true;
        }

        public Vector2 OnMove()
        {
            bool leftPressed = leftMoveButton.GetPressed();
            bool rightPressed = rightMoveButton.GetPressed();
            // if both left and right buttons are pressed or unpressed
            if (leftPressed == false && rightPressed == false)
                return Vector2.zero;
            return new Vector2((leftPressed? -1:0) + (rightPressed? 1:0), 0);
        }
    
        public bool OnClicked()
        {
            return functionButton.GetPressed();
        }
    }
}
