using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRodController : MonoBehaviour
{
    [SerializeField] LineRenderer Line;
	[SerializeField] GameObject Hook;
    [SerializeField] float HookVelocity;
    [SerializeField] float HookMaxDistance;

	Vector3 _moveDirection;

	private void Start()
	{
		Line.SetPosition(0, Line.transform.position);
	}

	private void Update()
	{
		_moveDirection = Vector2.zero;

		if(Input.GetKey(KeyCode.W))
		{
			_moveDirection += Vector3.up;
		}
		if(Input.GetKey(KeyCode.A))
		{
			_moveDirection += Vector3.left;
		}
		if(Input.GetKey(KeyCode.S))
		{
			_moveDirection += Vector3.down;
		}
		if(Input.GetKey(KeyCode.D))
		{
			_moveDirection += Vector3.right;
		}

		_moveDirection.Normalize();
	}

	private void FixedUpdate()
	{
		Vector3 displacement = Time.fixedDeltaTime * _moveDirection * HookVelocity;
		Vector3 newPosition = Hook.transform.position + displacement;

		if((Line.transform.position - newPosition).sqrMagnitude <= (HookMaxDistance * HookMaxDistance))
		{
			Hook.transform.position = newPosition;
			Line.SetPosition(1, Hook.transform.position);
		}
	}
}
