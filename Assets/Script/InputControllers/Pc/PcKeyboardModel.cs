using UnityEngine;

namespace InputControllers.Pc
{
    public class PcKeyboardModel : MonoBehaviour
    { 
        [SerializeField] public KeyCode clicked = KeyCode.Space;
        /*
         * 0 for up
         * 1 for down
         * 2 for left
         * 3 for right
         */
        [SerializeField] public KeyCode[] walk = new KeyCode[]
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D
        };

        public Vector2 GetMove()
        {
            var dir = new Vector2(0,0);
            /* does not allowed vertical movement
            if(Input.GetKey(walk[0]))
                dir += Vector2.up;
            if(Input.GetKey(walk[1]))
                dir += Vector2.down;
            */
            if(Input.GetKey(walk[2]))
                dir += Vector2.left;
            if(Input.GetKey(walk[3]))
                dir += Vector2.right;
            return dir.normalized;
        }
        public bool GetPressed()
        {
            return Input.GetKey(clicked);
        }
    }
}