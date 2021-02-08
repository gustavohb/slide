using UnityEngine;

public class GameUIController : MonoBehaviour
{

    [SerializeField] private QuitWindowController _quitWindowController;

    [SerializeField] private GameMenuUI _gameMenuUI;

    private GameObject _quitWindowGO;

    private void Start()
    {
        if (_quitWindowController != null)
        {
            _quitWindowGO = _quitWindowController.gameObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_quitWindowController != null && _quitWindowGO.activeSelf)
            {
                _quitWindowController.CloseWindow();
            }
            else if (_gameMenuUI != null && _gameMenuUI.isOpen)
            {
                _gameMenuUI.CloseMenu();
            }
            else
            {
                _quitWindowGO.SetActive(true);
            }
        }
    }
}
