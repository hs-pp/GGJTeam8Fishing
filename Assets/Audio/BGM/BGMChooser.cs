using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChooser : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
	[SerializeField] DayLibrary dayLibrary;

	private void Start()
	{
		audioSource.clip = dayLibrary.Days[GameStateManager.GetDay() - 1].FishingBGM;
		audioSource.Play();
	}
}
