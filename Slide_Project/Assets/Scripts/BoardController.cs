using UnityEngine;
using ScriptableObjectArchitecture;

public class BoardController : MonoBehaviour
{
    [Header("Board Colliders")]
    [SerializeField] private GameObject _horizontalColliders;
    [SerializeField] private GameObject _verticalColliders;

    [Header("Events")]
    [SerializeField] private GameEvent _moveRightEvent = default;
    [SerializeField] private GameEvent _moveLeftEvent = default;
    [SerializeField] private GameEvent _moveUpEvent = default;
    [SerializeField] private GameEvent _moveDownEvent = default;

    [Header("Variables")]
    [SerializeField] private FloatVariable _moveDuration = default;

    private void Start()
    {
        _moveRightEvent?.AddListener(MoveRight);
        _moveLeftEvent?.AddListener(MoveLeft);
        _moveUpEvent?.AddListener(MoveUp);
        _moveDownEvent?.AddListener(MoveDown);

        ActivateColliders();
    }

    private void MoveRight()
    {
        _verticalColliders.SetActive(false);
        Invoke("ActivateColliders", _moveDuration.Value);
    }

    private void MoveLeft()
    {
        _verticalColliders.SetActive(false);
        Invoke("ActivateColliders", _moveDuration.Value);
    }

    private void MoveUp()
    {
        _horizontalColliders.SetActive(false);
        Invoke("ActivateColliders", _moveDuration.Value);
    }

    private void MoveDown()
    {
        _horizontalColliders.SetActive(false);
        Invoke("ActivateColliders", _moveDuration.Value);
    }

    private void ActivateColliders()
    {
        _horizontalColliders.SetActive(true);
        _verticalColliders.SetActive(true);
    }

    private void OnDestroy()
    {
        _moveRightEvent?.RemoveListener(MoveRight);
        _moveLeftEvent?.RemoveListener(MoveLeft);
        _moveUpEvent?.RemoveListener(MoveUp);
        _moveDownEvent?.RemoveListener(MoveDown);
    }
}
