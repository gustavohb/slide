using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private float _levelSelectorDelay = 1f;

    [SerializeField] private Button _nextLevelButton;

    [SerializeField] private Button _previousLevelButton;

    [SerializeField] private IntVariable _maxLevelAchieved = default;

    [SerializeField] private IntGameEvent _loadLevelEvent = default;

    private int _levelIndexToLoad;
    private float _levelSelectorTimer;

    private int _currentLevelIndex;

    private void Start()
    {
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        _nextLevelButton.onClick.AddListener(() => LoadNextLevel());
        _previousLevelButton.onClick.AddListener(() => LoadPreviousLevel());

        _levelIndexToLoad = _currentLevelIndex;

        UpdateUI();
    }

    private void Update()
    {
        _levelSelectorTimer -= Time.deltaTime;

        if (_levelSelectorTimer <= 0 && _levelIndexToLoad != _currentLevelIndex)
        {
            _loadLevelEvent.Raise(_levelIndexToLoad);
        }
    }

    private void UpdateUI()
    {
        _levelText.text = "#" + (_levelIndexToLoad - 1);

        if (_levelIndexToLoad <= 1)
        {
            _previousLevelButton.interactable = false;
        }
        else
        {
            _previousLevelButton.interactable = true;
        }

        if (_levelIndexToLoad >= _maxLevelAchieved.Value)
        {
            _nextLevelButton.interactable = false;
        }
        else
        {
            _nextLevelButton.interactable = true;
        }
    }


    public void LoadNextLevel()
    {

        _levelSelectorTimer = _levelSelectorDelay;

        if (_levelIndexToLoad < _maxLevelAchieved.Value)
        {
            _levelIndexToLoad++;
        }

        UpdateUI();
    }

    public void LoadPreviousLevel()
    {
        _levelSelectorTimer = _levelSelectorDelay;
        if (_levelIndexToLoad > 1)
        {
            _levelIndexToLoad--;
        }

        UpdateUI();
    }

}

