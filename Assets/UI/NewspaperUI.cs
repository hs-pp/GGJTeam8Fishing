using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperUI : MonoBehaviour
{
	[SerializeField] CanvasGroup newspaperUI;
	[SerializeField] Animator newspaperAnim;
	[SerializeField] DayLibrary dayLibrary;
	[SerializeField] Image newsPaperImage;
	[SerializeField] private SceneLoader SceneLoader;

	private void Start()
	{
		newspaperUI.gameObject.SetActive(true);
		newspaperUI.alpha = 1f;
		newspaperAnim.Play("NewspaperSpin", -1, 0);
		newsPaperImage.sprite = dayLibrary.Days[GameStateManager.GetDay() - 1].NewspaperSprite;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			OnStartDayPressed();
		}
	}

	public void OnStartDayPressed()
	{
		if (GameStateManager.GetDay() == 10)
		{
			SceneLoader.LoadScene("Ending");

		}
		else
		{
			SceneLoader.LoadScene("Docks");
		}
	}
}
