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
            if (leftPressed ^ rightPressed)
                return Vector2.zero;
            Debug.Log("move");
            return leftPressed ? Vector2.left : Vector2.right;
        }
    
        public bool OnClicked()
        {
            Debug.Log("click");
            return functionButton.GetPressed();
        }
    }
}
