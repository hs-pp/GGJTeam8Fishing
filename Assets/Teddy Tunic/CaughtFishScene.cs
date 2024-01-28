using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtFishScene : MonoBehaviour
{
    [SerializeField] Transform fishHoldPoint;
	[SerializeField] HookController hookController;
	[SerializeField] float totalTime;

    enum State
    {
        IDLE,
        ACTIVE
    }

	State _currentState;
	FishInstance _fishInstance;
	float _currentTime;
	CameraFollower _cameraFollower;

	private void Start()
	{
		_currentState = State.IDLE;
		hookController.OnCatchFish += OnCatchFish;
		_currentTime = 0.0f;
		_cameraFollower = FindAnyObjectByType<CameraFollower>();
	}

	private void Update()
	{
		switch(_currentState)
		{
			case State.IDLE:
				// Do nothing
				break;
			case State.ACTIVE:
				// TODO: have dialogue play then we let hook controller resume
				_currentTime += Time.deltaTime;

				if(_currentTime >= totalTime)
				{
					hookController.SetState(HookController.State.PLAYER_CONTROLLED);
					_currentTime = 0.0f;
					_currentState = State.IDLE;
					_cameraFollower.ChangeTarget(hookController.transform);
					Destroy(_fishInstance.gameObject);
					_fishInstance = null;
				}
				break;
		}
	}

	private void OnCatchFish(FishInstance fishInstance)
	{
		_fishInstance = fishInstance;
		_fishInstance.SetState(FishState.Caught);
		_currentState = State.ACTIVE;
		_currentTime = 0.0f;
		_cameraFollower.ChangeTarget(_fishInstance.transform);

		fishInstance.transform.SetParent(fishHoldPoint);
		fishInstance.Rotate(new Vector3(0, 0, 90));
		fishInstance.transform.localPosition = Vector3.zero;
	}
}