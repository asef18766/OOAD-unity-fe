using Network;
using SocketIO;
using ThreadUtils;
using UnityEngine;
using UUID;

namespace InputControllers.Network.Server
{
    public class OperationEventListener : MonoBehaviour , IPlayerController
    {
        private Vector2 _curMove = Vector2.zero;
        private bool _clicked = false;
        public string PlayerID = "";
        public Vector2 OnMove()
        {
            if(_curMove == Vector2.zero)
                return Vector2.zero;
            var copy = _curMove.normalized;
            _curMove = Vector2.zero;
            return copy;
        }
        
        public bool OnClicked()
        {
            if (_clicked == false)
                return false;
            _clicked = true;
            return true;
        }
        private void Start()
        {
            var network = NetworkManager.GetInstance().GetComponent();
            PlayerID = gameObject.GetComponent<UuidObject>().uuid.ToString();
            network.On("operation" , _operation);
        }
        private void _operation(SocketIOEvent e)
        {
            var playerUuid = e.data["uuid"].str;
            if (playerUuid != PlayerID) return;
            UnityMainThread.Worker.AddJob(() =>
            {
                _curMove = new Vector2(e.data["move"].n , 0);
                _clicked = e.data["function"].b;
            });
        }
    }
}