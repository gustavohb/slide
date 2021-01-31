using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private class LoadingMonoBehaviour : MonoBehaviour { }

    private static AsyncOperation _loadingAsyncOperation;

    private static Action _onLoaderCallback;

    public static void Load(int sceneIndex)
    {
        // Set the loader callback action to load the target scene
        _onLoaderCallback = () =>
        {
            GameObject loadinGameObject = new GameObject("Loading Game Object");
            loadinGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(sceneIndex));
        };

        // Load the loading scene
        SceneManager.LoadScene("Loading");

    }

    private static IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return null;

        _loadingAsyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!_loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (_loadingAsyncOperation != null)
        {
            return _loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }

    public static void LoaderCallback()
    {
        // Triggered after the first Update which lets the screen refresh
        // Execute the loader callback action which will load the target scene
        _onLoaderCallback?.Invoke();
        _onLoaderCallback = null;
    }
}
