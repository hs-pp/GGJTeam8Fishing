using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishVision : MonoBehaviour
{
    [SerializeField]
    private FishRender m_fishRender;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // temp? logic to catch fish
        HookController hookController = collision.gameObject.GetComponent<HookController>();
        if(hookController != null)
        {
            m_fishRender.FishInstance.FishSeesHook(hookController.transform);
        }
    }
}