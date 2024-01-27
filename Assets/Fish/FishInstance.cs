using System;
using System.Collections;
using UnityEngine;

public class FishInstance : MonoBehaviour
{
    [SerializeField]
    private FishDefinition m_fishDefinition;
    [SerializeField]
    public string m_uniqueId;
    public string UniqueId => m_uniqueId;
    [SerializeField]
    private string m_fishName;
    [SerializeField]
    private float m_speed = 2f;
    private Vector3 m_destination;
    
    private FishRender m_fishRender;

    public void Awake()
    {
        m_fishRender = GetComponentInChildren<FishRender>();
        m_fishRender.SetFishInstance(this);
        StartCoroutine(PlayDialogueAfterWait());
    }
    
    private void Update()
    {
        SimpleMovement();
    }
    
    private void SimpleMovement()
    {
        // get new point
        if (Vector3.Magnitude(transform.position - m_destination) < 0.01f)
        {
            float randomX = UnityEngine.Random.Range(-10f, 10f);
            m_destination = new Vector3(randomX, 2, 0);
            //Debug.Log($"New Destination: {destination}");
        }
        
        // move towards point
        transform.position = Vector3.MoveTowards(transform.position, m_destination, m_speed * Time.deltaTime);

        // Inefficient to do this every frame but... eh.
        bool isMovingLeft = (transform.position.x - m_destination.x) < 0;
        if(isMovingLeft)
            m_fishRender.Rotate(new Vector3(0, 180, 0));
        else
            m_fishRender.Rotate(Vector3.zero);
    }
    
    public Vector3 GetCatchPoint()
    {
        return m_fishRender.GetCatchPoint();
    }
    
    private IEnumerator PlayDialogueAfterWait()
    {
        yield return new WaitForSeconds(3);
        m_fishRender.PlayDialogue("tf you looking at?");
    }

    public bool AutoSpawnDefinition(FishDefinition fishDefinition)
    {
        // kill all children
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        
        if (fishDefinition == null)
        {
            return false;
        }
        if (fishDefinition.FishRender == null)
        {
            Debug.LogError("FishDefinition has no FishRender!");
            return false;
        }
        
        // create new child
        GameObject child = Instantiate(fishDefinition.FishRender).gameObject;
        child.hideFlags = HideFlags.NotEditable;
        child.transform.SetParent(transform);
        child.transform.localPosition = Vector3.zero;
        return true;
    }
    
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void RefreshSpawns()
    {
        FishInstance[] fishInstances = FindObjectsOfType<FishInstance>();
        foreach (FishInstance fishInstance in fishInstances)
        {
            fishInstance.AutoSpawnDefinition(fishInstance.m_fishDefinition);
        }
    }
}
