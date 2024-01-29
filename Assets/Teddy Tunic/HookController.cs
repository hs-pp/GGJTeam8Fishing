using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HookController : MonoBehaviour
{
	[SerializeField] LineRender line; // Fishing rod line, don't confuse the name with LineRender(er)
	[SerializeField] GameObject hookPoint; // The point where the fish get snapped to
	[SerializeField] float velocity;
	[SerializeField] float maxDistance;
	[SerializeField] float autoreelVelocity;
	[SerializeField] float dropoffDistance; // Make this a tiny number
	[SerializeField] AudioSource reelingSFX;
	[SerializeField] List<AudioSource> fishBiteSFX;
	[SerializeField] List<AudioSource> fishCaughtSFX;

	public enum State
	{
		PLAYER_CONTROLLED,
		AUTOREELING,
		IDLE
	}

	Vector3 _startPosition;
	Vector3 _moveDirection;
	State _currentState;
	FishInstance _caughtFish;
	FishRender _caughtFishRender; // yea

	public Action<FishInstance> OnCatchFish;

	public void SetState(State state) => _currentState = state;

	private void Start()
	{
		_startPosition = transform.position;
		_currentState = State.PLAYER_CONTROLLED;

		GameStateManager.ResetBait();
	}

	private bool reelingIsPlaying = false;
	private void Update()
	{
		_moveDirection = Vector3.zero;

		switch (_currentState)
		{
			case State.PLAYER_CONTROLLED:
				bool isMoving = false;
				if (Input.GetKey(KeyCode.W))
				{
					_moveDirection += Vector3.up;
					isMoving = true;
				}
				if (Input.GetKey(KeyCode.A))
				{
					_moveDirection += Vector3.left;
					isMoving = true;
				}
				if (Input.GetKey(KeyCode.S))
				{
					_moveDirection += Vector3.down;
					isMoving = true;
				}
				if (Input.GetKey(KeyCode.D))
				{
					_moveDirection += Vector3.right;
					isMoving = true;
				}

				_moveDirection.Normalize();
				if (isMoving == true && reelingIsPlaying == false)
				{
					reelingSFX.Play();
					reelingIsPlaying = true;
				}
				else
				{
					reelingSFX.Stop();
					reelingIsPlaying = false;
				}
				
				break;

			case State.AUTOREELING:

				if (_caughtFish == null)
				{
					SetState(State.PLAYER_CONTROLLED);
					return;
				}
				
				Vector3 hookToStart = _startPosition - transform.position;

				// Fish is caught
				if (_startPosition == transform.position || hookToStart.sqrMagnitude <= (dropoffDistance * dropoffDistance))
				{
					reelingSFX.Stop();
					//fishCaughtSFX[UnityEngine.Random.Range(0, fishCaughtSFX.Count)].Play();
					GameStateManager.DecrementBait();
					OnCatchFish?.Invoke(_caughtFish);
					_caughtFishRender.SetAnimation("Caught");
					_caughtFishRender = null;
					_caughtFish = null;
					_currentState = State.IDLE;
				}
				else
				{
					_moveDirection = hookToStart.normalized;

					Vector3 displacement = _moveDirection * autoreelVelocity * Time.deltaTime;
					transform.position += displacement;
				}

				break;

			case State.IDLE:
				// Literally do nothing, let another script change its state
				break;
		}
	}


	private void FixedUpdate()
	{
		Vector3 displacement = _moveDirection * velocity * Time.fixedDeltaTime;
		Vector3 newPosition = transform.position + displacement;
		Vector3 hookToLine = line.transform.position - newPosition;

		// Update hook's position as long as it's new position is within max distance
		if (hookToLine.sqrMagnitude <= (maxDistance * maxDistance))
		{
			transform.position = newPosition;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		CatchPoint catchPoint = collision.gameObject.GetComponent<CatchPoint>();
		if (catchPoint != null && _caughtFish == null)
		{	
			_caughtFish = catchPoint.CatchFish(hookPoint.transform);
			_caughtFishRender = catchPoint.FishRender;
			_currentState = State.AUTOREELING;
			fishBiteSFX[UnityEngine.Random.Range(0, fishBiteSFX.Count)].Play();
			
			_currentState = State.AUTOREELING;
			reelingSFX.Play();
		}
	}
}
