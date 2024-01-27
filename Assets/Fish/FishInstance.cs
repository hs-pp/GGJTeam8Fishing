using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishState
{
    Swimming,
    Aware,
    Caught,
}

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
    private float m_speed = 10f;
    [SerializeField]
    private List<Vector3> m_waypoints;
    
    private FishRender m_fishRender;
    private FishState m_fishState = FishState.Swimming;

    public void Awake()
    {
        m_fishRender = GetComponentInChildren<FishRender>();
        m_fishRender.SetFishInstance(this);
        StartCoroutine(PlayDialogueAfterWait());
    }
    
    private void Update()
    {
        if (m_fishState == FishState.Swimming)
        {
            WaypointMovement();
        }

        LookTowardsMoveDirection();
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
        child.transform.SetParent(transform);
        child.transform.localPosition = Vector3.zero;
        child.hideFlags = HideFlags.NotEditable;
        for(int i = 0; i < child.transform.childCount; i++)
        {
            child.transform.GetChild(i).gameObject.hideFlags = HideFlags.NotEditable;
        }
        return true;
    }

    private int m_waypointIndex = 0;
    private void WaypointMovement()
    {
        if (m_waypoints.Count == 0)
        {
            return;
        }

        Vector3 destination = m_waypoints[m_waypointIndex];
        if (Vector3.Magnitude(transform.position - destination) < 0.01f)
        {
            m_waypointIndex++;
            if (m_waypointIndex >= m_waypoints.Count)
            {
                m_waypointIndex = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, m_speed * Time.deltaTime);
    }

    private Vector3 m_lastPosition;
    private void LookTowardsMoveDirection()
    {
        Vector3 newPosition = transform.position;
        Vector3 moveDirection = newPosition - m_lastPosition;
        if (moveDirection.magnitude > 0.01f)
        {
            if (moveDirection.x > 0)
            {
                m_fishRender.Rotate(new Vector3(0, 180, 0));
            }
            else
            {
                m_fishRender.Rotate(new Vector3(0, 0, 0));
            }
        }

        m_lastPosition = transform.position;
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
