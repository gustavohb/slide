using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EndGameMessageWindowController : MonoBehaviour
{
    [SerializeField] private string _closeWindowParameterID = "close";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
        _animator.SetBool(_closeWindowParameterID, false);
    }

    public void CloseWindow()
    {
        _animator.SetBool(_closeWindowParameterID, true);
    }


    private void OnDisable()
    {
        QuitApplication();
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
