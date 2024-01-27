using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDefinition", menuName = "FishDefinition")]
public class FishDefinition : ScriptableObject
{
    public string Species;
    public string Description;
    public FishRender FishRender;
    public int Value;
}
