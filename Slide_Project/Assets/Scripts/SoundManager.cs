using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        SliderMove,
        BlueSliderOut,
        PinkSliderOut,
        TutorialCompleted,
    }

    private static GameObject _oneShotGameObject;
    private static AudioSource _oneShotAudioSource;

    private static bool _isMuted;

    public static bool isMuted
    {
        get
        {
            if (!isInitialized) Initialize();
            return _isMuted;
        }
        set

        {
            _isMuted = value;
            PlayerPrefs.SetInt(isMutedPlayerPrefs, _isMuted ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private const string isMutedPlayerPrefs = "IS_MUTED_PLAYER_PREFS";

    private static bool isInitialized = false;

    public static void Initialize()
    {
        _isMuted = PlayerPrefs.GetInt(isMutedPlayerPrefs, 0) == 1;
        isInitialized = true;
    }

    public static void PlaySound(Sound sound)
    {
        if (!isInitialized)
        {
            Initialize();
        }
        if (_isMuted) return;
        if (_oneShotGameObject == null)
        {
            _oneShotGameObject = new GameObject("Sound");
            GameObject.DontDestroyOnLoad(_oneShotGameObject);
            _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
        }
        _oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClips[Random.Range(0, soundAudioClip.audioClips.Length)];
            }
        }
        Debug.LogError("Sound " + sound + "not found!");
        return null;
    }

    public static void ToggleSound()
    {
        isMuted = !isMuted;
    }
}
