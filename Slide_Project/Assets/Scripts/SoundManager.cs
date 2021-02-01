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

    public static void PlaySound(Sound sound)
    {
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

}
