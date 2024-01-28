using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

	private void Start()
	{
		LoadVolume("MasterVolume");
		LoadVolume("BGMVolume");
		LoadVolume("SFXVolume");
	}

	private void LoadVolume(string name)
	{
		if (PlayerPrefs.HasKey(name))
			audioMixer.SetFloat(name, SliderValueToVolume(PlayerPrefs.GetFloat(name)));
		else
			audioMixer.SetFloat(name, 0); // 0 will be the highest audio mixer volume
	}

	public void ChangeMasterVolume(float value)
	{
		SetAndStoreVolume("MasterVolume", value);
	}

	public void ChangeBGMVolume(float value)
    {
		SetAndStoreVolume("BGMVolume", value);
	}

	public void ChangeSFXVolume(float value)
    {
		SetAndStoreVolume("SFXVolume", value);
	}

	// Helper function to convert the [0.0001,1] slider value to the mixer's volume between [-80, 0]
	private float SliderValueToVolume(float value)
	{
		return Mathf.Log10(value) * 20;
	}

	private void SetAndStoreVolume(string name, float value)
	{
		audioMixer.SetFloat(name, SliderValueToVolume(value));
		PlayerPrefs.SetFloat(name, value);
	}
}
