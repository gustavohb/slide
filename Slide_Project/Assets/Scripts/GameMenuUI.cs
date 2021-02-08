using UnityEngine;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(Animator))]
public class GameMenuUI : MonoBehaviour
{
    [SerializeField] private BoolGameEvent _openCloseMenuEvent = default;

    [SerializeField] private GameEvent _reloadLevelEvent = default;

    [SerializeField] private Animator _animator;

    [SerializeField] private string _showMenuParameterID = "showMenu";

    [SerializeField] private GameObject _closeButton;
    [SerializeField] private GameObject _soundOnIcon;
    [SerializeField] private GameObject _soundOffIcon;

    [SerializeField] private GameObject _quitWindowGO;

    private bool _isMenuOpen = false;

    public bool isOpen
    {
        get { return _isMenuOpen; }
    }

    private void Start()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        _closeButton.SetActive(false);

        UpdateSoundButtonUI();
    }

    public void ToggleMenu()
    {
        if (!_isMenuOpen)
        {
            _openCloseMenuEvent.Raise(true);
            _animator?.SetBool(_showMenuParameterID, true);
            _closeButton.SetActive(true);
        }
        else
        {
            _openCloseMenuEvent.Raise(false);
            _animator?.SetBool(_showMenuParameterID, false);
            _closeButton.SetActive(false);
        }

        _isMenuOpen = !_isMenuOpen;
    }

    public void CloseMenu()
    {
        if(_isMenuOpen)
        {
            ToggleMenu();
        }
    }

    public void ReloadLevel()
    {
        _reloadLevelEvent?.Raise();
    }

    public void OpenQuitWindow()
    {
        _quitWindowGO?.SetActive(true);
    }

    public void ToggleSound()
    {
        SoundManager.ToggleSound();
        UpdateSoundButtonUI();
    }

    private void UpdateSoundButtonUI()
    {
        _soundOnIcon?.SetActive(!SoundManager.isMuted);
        _soundOffIcon?.SetActive(SoundManager.isMuted);
    }
}
