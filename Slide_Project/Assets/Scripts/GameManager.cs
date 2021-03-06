using UnityEngine;
using ScriptableObjectArchitecture;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameEvent _blueSliderOutEvent = default;
    [SerializeField] private GameEvent _pinkSliderOutEvent = default;
    [SerializeField] private GameEvent _levelCompletedEvent = default;
    [SerializeField] private GameEvent _reloadLevelEvent = default;

    [SerializeField] private IntVariable _maxLevelAchieved = default;

    private const string MAX_LEVEL_ACHIEVED_PLAYERPREFS = "maxLevelAchieved";

    private List<SliderController> _sliders;

    private int _blueSliderCount;

    private int _pinkSliderOut = 0;

    private void Start()
    {
        _maxLevelAchieved.Value = PlayerPrefs.GetInt(MAX_LEVEL_ACHIEVED_PLAYERPREFS, 0); ;

        SaveLevelPosition();

        _blueSliderOutEvent?.AddListener(OnBlueSliderOut);
        _pinkSliderOutEvent?.AddListener(OnPinkSliderOut);

        _sliders = new List<SliderController>(FindObjectsOfType<SliderController>());

        foreach (var slider in _sliders)
        {
            if (slider.CompareTag("BlueSlider"))
            {
                _blueSliderCount++;
            }
        }
    }

    private void OnBlueSliderOut()
    {
        _blueSliderCount--;

        if (LevelHasFinished())
        {
            Invoke("MoveToNextLevel", 0.5f);
        }
    }

    private void OnPinkSliderOut()
    {
        _pinkSliderOut++;

        Invoke("RestartLevel", 0.5f);
    }

    private bool LevelHasFinished()
    {
        return _blueSliderCount <= 0 && _pinkSliderOut == 0;
    }

    private void MoveToNextLevel()
    {
        SoundManager.PlaySound(SoundManager.Sound.LevelCompleted);
        _levelCompletedEvent?.Raise();
    }

    public void RestartLevel()
    {
        _reloadLevelEvent?.Raise();
    }

    private void SaveLevelPosition()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel > _maxLevelAchieved.Value)
        {
            PlayerPrefs.SetInt(MAX_LEVEL_ACHIEVED_PLAYERPREFS, currentLevel);
            PlayerPrefs.Save();
            _maxLevelAchieved.Value = currentLevel;
        }
    }

    private void OnDestroy()
    {
        _blueSliderOutEvent?.RemoveListener(OnBlueSliderOut);
        _pinkSliderOutEvent?.RemoveListener(OnPinkSliderOut);
    }
}

