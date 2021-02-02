using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Animator))]
public class GameMenuUI : MonoBehaviour
{
    [SerializeField] private BoolGameEvent _openCloseMenuEvent = default;

    [SerializeField] private GameEvent _reloadLevelEvent = default;

    [SerializeField] private Animator _animator;

    [SerializeField] private string _showMenuParamterID = "showMenu";

    [SerializeField] private GameObject _closeButton;

    private bool _isMenuOpen = false;

    private void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _closeButton.SetActive(false);
    }

    public void ToggleMenu()
    {
        if (!_isMenuOpen)
        {
            _openCloseMenuEvent.Raise(true);
            _animator?.SetBool(_showMenuParamterID, true);
            _closeButton.SetActive(true);
        }
        else
        {
            _openCloseMenuEvent.Raise(false);
            _animator?.SetBool(_showMenuParamterID, false);
            _closeButton.SetActive(false);
        }

        _isMenuOpen = !_isMenuOpen;
    }

    public void ReloadLevel()
    {
        _reloadLevelEvent?.Raise();
    }

    public void ShowQuitPopup()
    {
        //TODO
        Debug.Log("Show quit popup");
    }
}
