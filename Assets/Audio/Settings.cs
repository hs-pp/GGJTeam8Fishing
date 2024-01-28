using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

	private void Start()
	{
		masterSlider.value = TryGetVolume("MasterVolume");
		bgmSlider.value = TryGetVolume("BGMVolume");
		sfxSlider.value = TryGetVolume("SFXVolume");
	}

	private float TryGetVolume(string name)
	{
		if (PlayerPrefs.HasKey(name))
			return PlayerPrefs.GetFloat(name);
		else
			return 1; // 1 will be the slider's max default value
	}
}
