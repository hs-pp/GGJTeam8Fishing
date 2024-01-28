using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtFishScene : MonoBehaviour
{
    [SerializeField] Transform fishHoldPoint;
	[SerializeField] HookController hookController;
	[SerializeField] float totalTime;
	[SerializeField] DialogueScene dialogueScene;
	[SerializeField] Animator teddyAnim;
	[SerializeField] GameObject fishingRod;

	FishInstance _fishInstance;
	CameraFollower _cameraFollower;
	SceneLoader _sceneLoader;

	private void Start()
	{
		hookController.OnCatchFish += OnCatchFish;
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

	private void FinishScene()
    {
		fishingRod.SetActive(true);
		teddyAnim.Play("Idle");
		_cameraFollower.ChangeTarget(hookController.transform);
		Destroy(_fishInstance.gameObject);
		_fishInstance = null;

		if (GameStateManager.GetBaitAmount() == 0)
		{
			_sceneLoader.LoadNextScene();
		}
		else
		{
			hookController.SetState(HookController.State.PLAYER_CONTROLLED);
		}		
	}

}
