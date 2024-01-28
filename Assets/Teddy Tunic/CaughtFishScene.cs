using System.Collections;
using System.Collections.Generic;
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


	FishInstance _fishInstance;
	CameraFollower _cameraFollower;
	SceneLoader _sceneLoader;

	private void Start()
	{
		if (GameStateManager.GetDay() != 9)
		{
			hookController.OnCatchFish += OnCatchFish;
			evilJefferson.SetActive(false);
		}
		else
		{
			hookController.OnCatchFish += FlingFish;
		}

		_cameraFollower = FindObjectOfType<CameraFollower>();
		_sceneLoader = FindObjectOfType<SceneLoader>();
		dialogueScene.OnDialogueFinish += FinishScene;
	}
	private void OnCatchFish(FishInstance fishInstance)
	{
		_fishInstance = fishInstance;
		_fishInstance.SetState(FishState.Caught);
		_cameraFollower.ChangeTarget(_fishInstance.transform);
		fishInstance.transform.SetParent(fishHoldPoint);
		fishInstance.Rotate(new Vector3(0, 0, 90));
		fishInstance.transform.localPosition = Vector3.zero;

		fishingRod.SetActive(false);
		teddyAnim.Play("Hold");
		dialogueScene.NewScene(fishInstance.GetDialogueWhenCaught());
		
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
		Destroy(_fishInstance.gameObject);
		_fishInstance = null;

		if (GameStateManager.GetBaitAmount() == 0)
		{
			if(GameStateManager.GetDay() < GameStateManager.GetLastDay())
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

}
