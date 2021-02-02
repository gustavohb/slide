using UnityEngine;

public class IntroScreenController : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private float _loadLevelDelay = 4.0f;

    private void Start()
    {
        if (_levelLoader == null)
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
        }
        
        _levelLoader?.LoadLastAchievedLevel(_loadLevelDelay);
    }
}
