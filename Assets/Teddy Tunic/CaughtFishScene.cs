using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class CaughtFishScene : MonoBehaviour
{
    [SerializeField] Transform fishHoldPoint;
	[SerializeField] HookController hookController;
	[SerializeField] float totalTime;
	[SerializeField] DialogueScene dialogueScene;
	[SerializeField] Animator teddyAnim;
	[SerializeField] GameObject fishingRod;
	[SerializeField] PlayableDirector director;
	[SerializeField] private Transform flingFishTransform;
	[SerializeField] private DialogueBubble evilJeffersonBubble;
	[SerializeField] private GameObject evilJefferson;
	[SerializeField] private GameObject evilBackground;
	[SerializeField] AudioSource victoryMusic;

	[SerializeField]
	private List<DialogueItem> _day2Dialogue;
	[SerializeField]
	private List<DialogueItem> _day4Dialogue;
	[SerializeField]
	private List<DialogueItem> _day5Dialogue;
	[SerializeField]
	private List<DialogueItem> _day8Dialogue;

	FishInstance _fishInstance;
	CameraFollower _cameraFollower;
	SceneLoader _sceneLoader;

	private void Start()
	{
		if (GameStateManager.GetDay() != 9)
		{
			hookController.OnCatchFish += OnCatchFish;
			evilJefferson.SetActive(false);
			evilBackground.SetActive(false);
		}
		else
		{
			hookController.OnCatchFish += FlingFish;
			evilJefferson.SetActive(true);
			evilBackground.SetActive(true);
		}

		_cameraFollower = FindObjectOfType<CameraFollower>();
		_sceneLoader = FindObjectOfType<SceneLoader>();
		dialogueScene.OnDialogueFinish += FinishScene;
	}
	private async void OnCatchFish(FishInstance fishInstance)
	{
		_fishInstance = fishInstance;
		_fishInstance.SetState(FishState.Caught);
		_cameraFollower.ChangeTarget(_fishInstance.transform);
		fishInstance.transform.SetParent(fishHoldPoint);
		fishInstance.Rotate(new Vector3(0, 0, 90));
		fishInstance.transform.localPosition = Vector3.zero;

		fishingRod.SetActive(false);
		teddyAnim.Play("Hold");

		await Task.Delay(200);
		if (GameStateManager.GetDay() == 2 && GameStateManager.GetBaitAmount() == 0)
		{
			dialogueScene.NewScene(_day2Dialogue, 1);
		}
		else if (GameStateManager.GetDay() == 4 && GameStateManager.GetBaitAmount() == 2)
		{
			dialogueScene.NewScene(_day4Dialogue, 1);
		}
		else if (GameStateManager.GetDay() == 5 && GameStateManager.GetBaitAmount() == 0)
		{
			dialogueScene.NewScene(_day5Dialogue, 1);
		}
		else if (GameStateManager.GetDay() == 8)
		{
			dialogueScene.OnDialogueFinish -= FinishScene;
			dialogueScene.OnDialogueFinish += InstantFinishScene;
			dialogueScene.NewScene(_day8Dialogue);
			dialogueScene.OnDialogueFinish -= FinishScene;
			dialogueScene.OnDialogueFinish += Day8FinishScene;
		}
		else
		{
			dialogueScene.NewScene(fishInstance.GetDialogueWhenCaught());
		}
	}

	private void FlingFish(FishInstance fishInstance)
	{
		_fishInstance = fishInstance;
		_fishInstance.SetState(FishState.Caught);
		fishInstance.transform.SetParent(flingFishTransform);
		fishInstance.transform.localPosition = Vector3.zero;
		_cameraFollower.ChangeTarget(_fishInstance.transform);
		
		director.Play();
		director.stopped += FinishFling;
	}

	private void FinishFling(PlayableDirector pd)
	{
		if (GameStateManager.GetBaitAmount() == 0)
		{
			evilJeffersonBubble.Show(true);
			GameStateManager.IncrementDay();
			if (victoryMusic) victoryMusic.Play();
			_sceneLoader.LoadScene("Newspaper");
			return;
		}
		
		fishingRod.SetActive(true);
		teddyAnim.Play("Idle");
		_cameraFollower.ChangeTarget(hookController.transform);
		Destroy(_fishInstance.gameObject);
		_fishInstance = null;
		hookController.SetState(HookController.State.PLAYER_CONTROLLED);
		director.stopped -= FinishFling;
	}

	private void FinishScene()
    {
		fishingRod.SetActive(true);
		teddyAnim.Play("Idle");
		_cameraFollower.ChangeTarget(hookController.transform);
		
		GameStateManager.AddCaughtFish(_fishInstance.UniqueName);
		Destroy(_fishInstance.gameObject);
		_fishInstance = null;

		if (GameStateManager.GetBaitAmount() == 0)
		{
			if (victoryMusic) victoryMusic.Play();

			if (GameStateManager.GetDay() < GameStateManager.GetLastDay())
			{
				GameStateManager.IncrementDay();
				_sceneLoader.LoadScene("Newspaper");
			}
			else
			{
				_sceneLoader.LoadScene("Main Menu"); // Prolly should be a credits scene once we have that
			}

		}
		else
		{
			hookController.SetState(HookController.State.PLAYER_CONTROLLED);
		}		
	}

	private void Day8FinishScene()
	{
		if (victoryMusic) victoryMusic.Play();
		GameStateManager.IncrementDay();
		_sceneLoader.LoadScene("Newspaper");
	}

	private void InstantFinishScene()
	{
		if (victoryMusic) victoryMusic.Play();
		_sceneLoader.LoadScene("Newspaper");
	}

}
