using UnityEngine;
using ScriptableObjectArchitecture;

public class ExitHoleController : MonoBehaviour
{
    [SerializeField] private GameEvent _blueSliderOutEvent = default;
    [SerializeField] private GameEvent _pinkSliderOutEvent = default;

    [SerializeField] private string _blueSliderTag = "BlueSlider";
    [SerializeField] private string _pinkSliderTag = "PinkSlider";

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == _blueSliderTag)
		{
			col.GetComponent<SliderController>()?.SetIsOutTrue();
			SoundManager.PlaySound(SoundManager.Sound.BlueSliderOut);
			_blueSliderOutEvent?.Raise();
		}
		else if (col.tag == _pinkSliderTag)
		{
			col.GetComponent<SliderController>()?.SetIsOutTrue();
			SoundManager.PlaySound(SoundManager.Sound.PinkSliderOut);
			_pinkSliderOutEvent?.Raise();
		}
	}
}
