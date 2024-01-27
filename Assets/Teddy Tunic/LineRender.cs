using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LineRender : MonoBehaviour
{
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] HookController hookController;

	private void Update()
	{
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, hookController.transform.position);
	}
}
