using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInstance : MonoBehaviour
{
    [SerializeField]
    private FishDefinition m_fishDefinition;
    [SerializeField]
    public string m_guid = Guid.NewGuid().ToString(); // make this readonly
    [SerializeField]
    private string m_fishName;
    
    
    // TODO: Fish behavior
    private void Update()
    {
        
    }
}
