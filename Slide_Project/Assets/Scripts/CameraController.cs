using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _viewArea;
    [SerializeField] private BoolGameEvent _openCloseMenuEvent = default;
    [SerializeField] private float _shrinkFactor = 5f;
    [SerializeField] private float _shrinkDuration = 1f;
    [SerializeField] private Ease _shrinkEase = Ease.OutBack;
    private Camera _camera;

    private float _originalOrthoSize;

    private void Start()
    {
        _camera = GetComponent<Camera>();

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = (_viewArea.bounds.size.x) / (_viewArea.bounds.size.y);

        
        if (screenRatio >= targetRatio)
        {
            _camera.orthographicSize = _viewArea.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            _camera.orthographicSize = _viewArea.bounds.size.y / 2 * differenceInSize;
        }

        _originalOrthoSize = _camera.orthographicSize;

        _openCloseMenuEvent.AddListener(ChangeCameraOrthograficSize);

    }

    private void ChangeCameraOrthograficSize(bool shrinkScreen)
    {
        float changeOrthoSizeTo;
        if (shrinkScreen)
        {
            changeOrthoSizeTo = _originalOrthoSize + _shrinkFactor;
        }
        else
        {
            changeOrthoSizeTo = _originalOrthoSize;
        }

        DOTween.To(() => _camera.orthographicSize, x => _camera.orthographicSize = x, changeOrthoSizeTo, _shrinkDuration).SetEase(_shrinkEase).Play();
    }
}
