using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchPoint : MonoBehaviour
{
    [SerializeField]
    private FishRender m_fishRender;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Triggered by {collision.gameObject.name}. I am {m_fishRender.FishInstance.name} ");
        
    }
}
