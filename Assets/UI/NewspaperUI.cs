using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewspaperUI : MonoBehaviour
{
	[SerializeField] CanvasGroup newspaperUI;
	[SerializeField] Animator newspaperAnim;
	[SerializeField] TextMeshProUGUI titleText;
	[SerializeField] TextMeshProUGUI bodyText;
	[SerializeField] DayLibrary dayLibrary;

	private void Start()
	{
		newspaperUI.gameObject.SetActive(true);
		newspaperUI.alpha = 1f;
		newspaperAnim.Play("NewspaperSpin", -1, 0);
		titleText.text = dayLibrary.Days[GameStateManager.GetDay() - 1].NewspaperTitle;
		bodyText.text = dayLibrary.Days[GameStateManager.GetDay() - 1].NewspaperBody;
	}
}
