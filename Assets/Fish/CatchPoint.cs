using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPoint : MonoBehaviour
{
    [SerializeField]
    private FishRender m_fishRender;

	public FishRender FishRender => m_fishRender;

    public FishInstance CatchFish(Transform hookPoint)
    {
		m_fishRender.FishInstance.transform.SetParent(hookPoint);
		m_fishRender.FishInstance.SetState(FishState.Reeling);
		m_fishRender.FishInstance.Rotate(new Vector3(0, 0, -90));
		Vector3 offset = transform.rotation * transform.localPosition;
		m_fishRender.FishInstance.transform.position = hookPoint.position - offset;

		return m_fishRender.FishInstance;
	}
}
