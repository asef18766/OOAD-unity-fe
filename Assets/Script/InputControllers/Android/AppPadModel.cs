using UnityEngine;

namespace InputControllers.Android
{
    public class AppPadModel : MonoBehaviour
    {
        private Bounds _bounds;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _bounds = gameObject.GetComponent<SpriteRenderer>().bounds;
        }

        private Vector2 _lastMove;
        private Camera _camera;

        public Vector2 GetMove()
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Stationary)
                    continue;
                if (_camera == null)
                    break;
                var touchBegin = _camera.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                var touchEnd = _camera.ScreenToWorldPoint(touch.position);
                touchBegin.z = _bounds.center.z;
                touchEnd.z = _bounds.center.z;
                if (_bounds.Contains(touchBegin) && _bounds.Contains(touchEnd))
                {
                    if (touch.phase == TouchPhase.Moved)
                        _lastMove = touch.deltaPosition.normalized;
                    return _lastMove;
                }
            }
            _lastMove = Vector2.zero;
            return Vector2.zero;
        }

        public bool GetPressed()
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Stationary && touch.phase != TouchPhase.Moved)
                    continue;
                if (_camera == null)
                    break;
                var touching = _camera.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                touching.z = _bounds.center.z;
                if (_bounds.Contains(touching))
                    return true;
            }
            return false;
        }
    }
}