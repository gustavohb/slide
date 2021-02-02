using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator _transition;

    [SerializeField] private float _transitionTime = 0.5f;

    [SerializeField] private GameEvent _levelCompletedEvent = default;

    [SerializeField] private GameEvent _reloadLevelEvent = default;

    [SerializeField] private IntGameEvent _loadLevelEvent = default;

    private const string MAX_LEVEL_ACHIEVED_PLAYERPREFS = "maxLevelAchieved";

    private void Start()
    {
        _levelCompletedEvent?.AddListener(LoadNextLevel);
        _reloadLevelEvent?.AddListener(ReloadLevel);
        _loadLevelEvent?.AddListener(LoadLevel);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevelCoroutine(int levelIndex, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);


        _transition?.SetTrigger("Start");


        yield return new WaitForSeconds(_transitionTime);

        Loader.Load(levelIndex);
    }

    public void ReloadLevel()
    {
        StartCoroutine(LoadLevelCoroutine(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLastAchievedLevel(float delay = 0f)
    {
        int currentLevel = PlayerPrefs.GetInt(MAX_LEVEL_ACHIEVED_PLAYERPREFS, 1);
        StartCoroutine(LoadLevelCoroutine(currentLevel, delay));
    }

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex, 0.0f));
    }

    public void LoadLevel(int levelIndex, float delay)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex, delay));
    }

    private void OnDestroy()
    {
        _levelCompletedEvent?.RemoveListener(LoadNextLevel);
        _reloadLevelEvent?.RemoveListener(ReloadLevel);
        _loadLevelEvent?.RemoveListener(LoadLevel);
    }

}
