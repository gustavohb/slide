using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _viewArea;

    private Camera _camera;

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
    }
}
