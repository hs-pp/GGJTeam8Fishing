using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperUI : MonoBehaviour
{
	[SerializeField] CanvasGroup newspaperUI;
	[SerializeField] Animator newspaperAnim;

	private void Start()
	{
		newspaperUI.gameObject.SetActive(true);
		newspaperUI.alpha = 1f;
		newspaperAnim.Play("NewspaperSpin", -1, 0);
	}
}
