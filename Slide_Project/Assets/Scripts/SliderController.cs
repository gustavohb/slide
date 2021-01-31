using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class SliderController : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private FloatVariable _moveDuration = default;

    [Header("Events")]
    [SerializeField] private GameEvent _moveRightEvent = default;
    [SerializeField] private GameEvent _moveLeftEvent = default;
    [SerializeField] private GameEvent _moveUpEvent = default;
    [SerializeField] private GameEvent _moveDownEvent = default;

    private float _dirX = 0;
    private float _dirY = 0;

    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private const float _moveForce = 1000;
    
    public bool _isOut { private set; get; }
    private bool _isMoving;
    private float _moveTimer;

    private Vector3 _stopPosition;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _isMoving = false;

        _animator = GetComponent<Animator>();

        _isOut = false;

        _stopPosition = transform.position;

        _moveRightEvent?.AddListener(MoveRight);
        _moveLeftEvent?.AddListener(MoveLeft);
        _moveUpEvent?.AddListener(MoveUp);
        _moveDownEvent?.AddListener(MoveDown);
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
        {
            transform.position = _stopPosition;
        }
        else
        {
            _rigidbody.AddForce(new Vector2(_dirX, _dirY));
        }
    }

    private void Update()
    {
        if (_isOut)
        {
            _rigidbody.velocity = new Vector2(0, 0);

            _animator?.SetBool("IsOut", _isOut);
        }

        if (_isMoving) _moveTimer -= Time.deltaTime;

        if (_moveTimer <= 0)
        {
            if (_isMoving || !_isOut)
            {
                StopMove();
                _stopPosition = transform.position;
            }
        }
    }

    public void MoveRight()
    {
        if (_isOut || _isMoving) return;
        _dirX = _moveForce;
        _dirY = 0;
        _isMoving = true;
        _moveTimer = _moveDuration.Value;
    }

    public void MoveLeft()
    {
        if (_isOut || _isMoving) return;
        _dirX = -_moveForce;
        _dirY = 0;
        _isMoving = true;
        _moveTimer = _moveDuration.Value;
    }

    public void MoveUp()
    {
        if (_isOut || _isMoving) return;
        _dirX = 0;
        _dirY = _moveForce;
        _isMoving = true;
        _moveTimer = _moveDuration.Value;
    }

    public void MoveDown()
    {
        if (_isOut || _isMoving) return;
        _dirX = 0;
        _dirY = -_moveForce;
        _isMoving = true;
        _moveTimer = _moveDuration.Value;
    }

    public void StopMove()
    {
        _dirX = 0;
        _dirY = 0;
        _stopPosition = transform.position;
        _isMoving = false;
    }

    public void SetIsOutTrue()
    {
        _isOut = true;
        _stopPosition = transform.position;
        StopMove();
    }

    private void OnDestroy()
    {
        _moveRightEvent?.RemoveListener(MoveRight);
        _moveLeftEvent?.RemoveListener(MoveLeft);
        _moveUpEvent?.RemoveListener(MoveUp);
        _moveDownEvent?.RemoveListener(MoveDown);
    }
}
