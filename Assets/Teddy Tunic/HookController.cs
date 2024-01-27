using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
	[SerializeField] LineRender Line; // Fishing rod line, don't confuse the name with LineRender(er)
	[SerializeField] GameObject HookPoint; // The point where the fish get snapped to
	[SerializeField] float HookVelocity;
	[SerializeField] float HookMaxDistance;

	Vector3 _moveDirection;

	private void Update()
	{
		_moveDirection = Vector2.zero;

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
	}

	private void FixedUpdate()
	{
		Vector3 displacement =  _moveDirection * HookVelocity * Time.fixedDeltaTime;
		Vector3 newPosition = transform.position + displacement;
		Vector3 hookToLine = Line.transform.position - newPosition;

		// Update hook's position as long as it's new position is within max distance
		if (hookToLine.sqrMagnitude <= (HookMaxDistance * HookMaxDistance))
		{
			transform.position = newPosition;
		}
	}
}
