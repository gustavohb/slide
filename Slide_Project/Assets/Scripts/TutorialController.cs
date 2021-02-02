using UnityEngine;
using ScriptableObjectArchitecture;

public class TutorialController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _step1GO;
    [SerializeField] private GameObject _step2GO;
    [SerializeField] private GameObject _step3GO;

    [SerializeField] private TutorialMessageWindow _rulesMessageWindow;
    [SerializeField] private TutorialMessageWindow _congratulationsMessageWindow;

    [SerializeField] private Camera _targetCamera;

    [Header("Variables")]
    [SerializeField] private FloatVariable _moveDuration = default;

    [Header("Events")]
    [SerializeField] private GameEvent _moveRightEvent = default;
    [SerializeField] private GameEvent _moveLeftEvent = default;
    [SerializeField] private GameEvent _moveUpEvent = default;
    [SerializeField] private GameEvent _moveDownEvent = default;
    [SerializeField] private GameEvent _levelCompletedEvent = default;

    private int _currentStep = 0;
    private bool _stepFinished = false;
    private float _moveTimer = 0;
    private bool _isMoving = false;
    private int _touchId = 99;
    private bool _touchStart = false;
    private Vector2 _touchPointA;
    private Vector2 _touchPointB;

    private const float _minMoveTouchDistance = 0.5f;
    private float _messageWindowDelayTime = 1f;
    private float _animationStepDelayTime = .1f;
    private float _messageWindowTimer = 0f;
    private float _animationStepTimer = 0f;
    private bool _messageWindowIsOpen = false;

    private void Start()
    {
        if (_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }
        
        _messageWindowTimer = _messageWindowDelayTime;
        _animationStepTimer = _animationStepDelayTime;

        Invoke("OpenMessageWindow", _messageWindowDelayTime);
    }

    private void Update()
    {
        _messageWindowTimer -= Time.deltaTime;
        _animationStepTimer -= Time.deltaTime;

        if (_messageWindowTimer <= 0 && _stepFinished)
        {
            OpenMessageWindow();
            _stepFinished = false;
        }
        if (_animationStepTimer <= 0 && !_messageWindowIsOpen)
        {
            switch (_currentStep)
            {
                case 0:
                    _step1GO.SetActive(false);
                    _step2GO.SetActive(false);
                    _step3GO.SetActive(false);
                    break;
                case 1:
                    _step1GO.SetActive(true);
                    _step2GO.SetActive(false);
                    _step3GO.SetActive(false);
                    break;
                case 2:
                    _step1GO.SetActive(false);
                    _step2GO.SetActive(true);
                    _step3GO.SetActive(false);
                    break;
                case 3:
                    _step1GO.SetActive(false);
                    _step2GO.SetActive(false);
                    _step3GO.SetActive(true);
                    break;
                default:
                    _step1GO.SetActive(false);
                    _step2GO.SetActive(false);
                    _step3GO.SetActive(false);
                    break;
            }
        }

        if (_stepFinished)
        {
            _step1GO.SetActive(false);
            _step2GO.SetActive(false);
            _step3GO.SetActive(false);
        }

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

        _isMoving = _moveTimer > 0;
    }


    private void FixedUpdate()
    {
        if (_messageWindowIsOpen) return;

        if (_touchStart && !_isMoving)
        {
            Vector2 offset = _touchPointB - _touchPointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);

            float absXDir = Mathf.Abs(direction.x);
            float absYDir = Mathf.Abs(direction.y);

            if (absXDir > absYDir && absXDir >= _minMoveTouchDistance)
            {
                _moveTimer = _moveDuration.Value;

                if (direction.x < 0 && _currentStep == 2)
                {
                    _moveLeftEvent?.Raise();
                    SoundManager.PlaySound(SoundManager.Sound.SliderMove);
                    //step = 3;
                    _currentStep++;
                    _messageWindowTimer = _messageWindowDelayTime;
                    _stepFinished = true;

                }
                else
                {
                    SoundManager.PlaySound(SoundManager.Sound.InvalidMovement);
                }

                _touchStart = false;
            }
            else if (absYDir > absXDir && absYDir >= _minMoveTouchDistance)
            {
                _moveTimer = _moveDuration.Value;

                if (direction.y > 0 && _currentStep == 1)
                {
                    _moveUpEvent?.Raise();
                    SoundManager.PlaySound(SoundManager.Sound.SliderMove);
                    _messageWindowTimer = _messageWindowDelayTime;
                    //step = 2;
                    _currentStep++;
                    _stepFinished = true;
                }
                else if (direction.y < 0 && _currentStep == 3)
                {
                    _moveDownEvent?.Raise();
                    SoundManager.PlaySound(SoundManager.Sound.SliderMove);
                    _messageWindowTimer = _messageWindowDelayTime;
                    //step = 4;
                    _currentStep++;
                    _stepFinished = true;
                }
                else
                {
                    SoundManager.PlaySound(SoundManager.Sound.InvalidMovement);
                }
                _touchStart = false;
            }

        }
    }

    private Vector2 TouchToWorldPoint(Vector2 touchPosition)
    {
        return _targetCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, _targetCamera.transform.position.z));
    }

    public void OpenMessageWindow()
    {
        if (_currentStep == 0)
        {
            _rulesMessageWindow.OpenWindow();
            _messageWindowIsOpen = true;
        }

        if (_currentStep > 3)
        {
            _congratulationsMessageWindow.OpenWindow();
            SoundManager.PlaySound(SoundManager.Sound.LevelCompleted);
            _messageWindowIsOpen = true;
        }
    }

    public void CloseMessageWindow()
    {
        if (_rulesMessageWindow.gameObject.activeSelf)
        {
            _rulesMessageWindow.CloseWindow();
        }

        if (_congratulationsMessageWindow.gameObject.activeSelf)
        {
            _congratulationsMessageWindow.CloseWindow();
            _levelCompletedEvent?.Raise();
        }
        
        _messageWindowIsOpen = false;
        _currentStep++;
        _animationStepTimer = _animationStepDelayTime;
    }
}
