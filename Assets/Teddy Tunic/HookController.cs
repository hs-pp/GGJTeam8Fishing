using System;
using UnityEngine;

public class HookController : MonoBehaviour
{
	[SerializeField] LineRender line; // Fishing rod line, don't confuse the name with LineRender(er)
	[SerializeField] GameObject hookPoint; // The point where the fish get snapped to
	[SerializeField] float velocity;
	[SerializeField] float maxDistance;
	[SerializeField] float autoreelVelocity;
	[SerializeField] float dropoffDistance; // Make this a tiny number

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

	public Action<FishInstance> OnCatchFish;

	public void SetState(State state) => _currentState = state;

	private void Start()
	{
		_startPosition = transform.position;
		_currentState = State.PLAYER_CONTROLLED;
	}

	private void Update()
	{
		_moveDirection = Vector3.zero;

		switch (_currentState)
		{
			case State.PLAYER_CONTROLLED:

				if (Input.GetKey(KeyCode.W))
				{
					_moveDirection += Vector3.up;
				}
				if (Input.GetKey(KeyCode.A))
				{
					_moveDirection += Vector3.left;
				}
				if (Input.GetKey(KeyCode.S))
				{
					_moveDirection += Vector3.down;
				}
				if (Input.GetKey(KeyCode.D))
				{
					_moveDirection += Vector3.right;
				}

				_moveDirection.Normalize();

				break;

			case State.AUTOREELING:

				Vector3 hookToStart = _startPosition - transform.position;

				// Fish is caught
				if (_startPosition == transform.position || hookToStart.sqrMagnitude <= (dropoffDistance * dropoffDistance))
				{
					GameStateManager.DecrementBait();
					OnCatchFish?.Invoke(_caughtFish);
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
			_currentState = State.AUTOREELING;
		}
	}
}
