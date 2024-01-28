using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FishingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI baitAmountText;
	[SerializeField] CanvasGroup mainUI;
	[SerializeField] CanvasGroup settingsUI;
	[SerializeField] CanvasGroup pauseUI;

	bool _isPaused;

	private void Start()
	{
		_isPaused = false;
	}

	private void Update()
	{
		moneyText.text = "Money: " + GameStateManager.GetMoney().ToString();
		baitAmountText.text = "Bait: " + GameStateManager.GetBaitAmount().ToString();

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseOrUnPause();
		}
	}

	private void PauseOrUnPause()
	{
		if (_isPaused)
		{
			pauseUI.gameObject.SetActive(false);
			pauseUI.alpha = 0f;
			mainUI.gameObject.SetActive(true);
			mainUI.alpha = 1f;
		}
		else
		{
			pauseUI.gameObject.SetActive(true);
			pauseUI.alpha = 1f;
			mainUI.gameObject.SetActive(false);
			mainUI.alpha = 0f;
		}

		_isPaused = !_isPaused;
	}
}
