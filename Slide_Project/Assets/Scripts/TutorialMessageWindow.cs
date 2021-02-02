using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialMessageWindow : MonoBehaviour
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
        Debug.Log("Close Window");
        _animator.SetBool(_closeWindowParameterID, true);
    }
}
