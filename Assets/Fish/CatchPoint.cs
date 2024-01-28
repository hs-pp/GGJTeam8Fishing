using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPoint : MonoBehaviour
{
    [SerializeField]
    private FishRender m_fishRender;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // temp? logic to catch fish
        HookController hookController = collision.gameObject.GetComponent<HookController>();
        if(hookController != null)
        {
            Transform hookPoint = hookController.GetHookPoint().transform;
            m_fishRender.FishInstance.transform.SetParent(hookPoint);
            m_fishRender.FishInstance.SetState(FishState.Reeling);
            m_fishRender.FishInstance.Rotate(new Vector3(0, 0, -90));
            Vector3 offset = transform.rotation * transform.localPosition;
            m_fishRender.FishInstance.transform.position = hookPoint.position - offset;
        }
    }
}
