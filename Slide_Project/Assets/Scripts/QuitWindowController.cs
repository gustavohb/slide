using UnityEngine;

[RequireComponent(typeof(Animator))]
public class QuitWindowController : MonoBehaviour
{
    [SerializeField] private string _closeQuitWindowParameterID = "close";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void CloseWindow()
    {
        _animator?.SetBool(_closeQuitWindowParameterID, true);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}
