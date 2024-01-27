using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishDefinition", menuName = "FishDefinition")]
[Serializable]
public class FishDefinition : ScriptableObject
{
    public string Species;
    public string Description;
    public FishRender FishRender;
    public int Value;
}
