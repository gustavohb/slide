using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator _transition;

    [SerializeField] private float _transitionTime = 0.5f;

    [SerializeField] private GameEvent _levelCompletedEvent = default;

    private void Start()
    {
        _levelCompletedEvent?.AddListener(LoadNextLevel);
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

    public void LoadCurrentLevel(float delay = 0f)
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        StartCoroutine(LoadLevelCoroutine(currentLevel, delay));
    }

    public void LoadLevel(int levelIndex, float delay = 0f)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex, delay));
    }

    private void OnDestroy()
    {
        _levelCompletedEvent?.RemoveListener(LoadNextLevel);
    }

}
