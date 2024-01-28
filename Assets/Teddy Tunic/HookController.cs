using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
	[SerializeField] LineRender Line; // Fishing rod line, don't confuse the name with LineRender(er)
	[SerializeField] GameObject HookPoint; // The point where the fish get snapped to
	[SerializeField] float Velocity;
	[SerializeField] float MaxDistance;

	//Code to reel in fish
	[SerializeField] float reelSpeed = 0.1f;
	Vector3 originalPos;
	Vector3 posDifference;
	bool reeling = false;

	Vector3 _moveDirection;
	
	public Transform GetHookPoint()
	{
		return HookPoint.transform;
	}

    private void Start()
    {
        originalPos = transform.position;
    }



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

		//Reeling
		if (Input.GetKeyDown(KeyCode.Space))
        {
			reeling = true;
        }


		if(reeling)
        {
			posDifference = originalPos - transform.position;
			transform.position += Vector3.ClampMagnitude(posDifference, reelSpeed);
			if (posDifference.magnitude < 1)
				reeling = false;
        }
	}

	private void FixedUpdate()
	{
		Vector3 displacement =  _moveDirection * Velocity * Time.fixedDeltaTime;
		Vector3 newPosition = transform.position + displacement;
		Vector3 hookToLine = Line.transform.position - newPosition;

		// Update hook's position as long as it's new position is within max distance
		if (hookToLine.sqrMagnitude <= (MaxDistance * MaxDistance))
		{
			transform.position = newPosition;
		}
	}
}
