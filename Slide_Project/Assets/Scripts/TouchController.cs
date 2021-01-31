using UnityEngine;
using ScriptableObjectArchitecture;

public class TouchController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveDuration = 0.5f;
    [SerializeField] private float _minMoveTouchDistance = 0.5f;

    [Header("References")]
    [SerializeField] private Camera _targetCamera;

    [Header("Events")]
    [SerializeField] private GameEvent _moveRightEvent = default;
    [SerializeField] private GameEvent _moveLeftEvent = default;
    [SerializeField] private GameEvent _moveUpEvent = default;
    [SerializeField] private GameEvent _moveDownEvent = default;

    private float _moveTimer = 0;
    private bool _isMoving = false;
    private int _touchId = 99;
    private bool _touchStart = false;
    private Vector2 _touchPointA;
    private Vector2 _touchPointB;

    private void Start()
    {
        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }
        
    }

    private void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = TouchToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began && _touchId == 99)
            {
                _touchStart = true;
                _touchPointA = touchPos;
                _touchPointB = touchPos;

                _touchId = touch.fingerId;

            }
            else if (touch.phase == TouchPhase.Moved && _touchId == touch.fingerId)
            {
                _touchPointB = touchPos;
            }
            else if (touch.phase == TouchPhase.Ended && _touchId == touch.fingerId)
            {
                _touchId = 99;
                _touchStart = false;
            }
            i++;
        }

        _moveTimer -= Time.deltaTime;

        if (_moveTimer > 0)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

    }

    private void FixedUpdate()
    {
        if (_touchStart && !_isMoving)
        {
            Vector2 offset = _touchPointB - _touchPointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            float absXDir = Mathf.Abs(direction.x);
            float absYDir = Mathf.Abs(direction.y);

            if (absXDir > absYDir && absXDir >= _minMoveTouchDistance)
            {
                _moveTimer = _moveDuration;

                if (direction.x > 0)
                {
                    _moveRightEvent?.Raise();
                }
                else
                {
                    _moveLeftEvent?.Raise();
                }

                _touchStart = false;
            }
            else if (absYDir > absXDir && absYDir >= _minMoveTouchDistance)
            {
                _moveTimer = _moveDuration;

                if (direction.y > 0)
                {
                    _moveUpEvent?.Raise();
                }
                else
                {
                    _moveDownEvent?.Raise();
                }

                _touchStart = false;
            }

        }
    }

    private Vector2 TouchToWorldPoint(Vector2 touchPosition)
    {
        return _targetCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, _targetCamera.transform.position.z));
    }

}
