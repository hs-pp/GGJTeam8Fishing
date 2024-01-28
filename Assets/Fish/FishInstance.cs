using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FishState
{
    Idle,
    Runaway,
    Reeling,
    Caught
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
    public string FishName => m_fishName;
    [SerializeField]
    private float m_speed = 1f;
    [SerializeField]
    private List<Vector3> m_waypoints;
    [SerializeField]
    private DialogueConfigWhenIdle m_idleDialogues;
    [SerializeField]
    private DialogueConfigFishWasCaught m_fishWasCaughtDialogues;
    [SerializeField]
    private DialogueConfigRunaway m_dialogueConfigRunaways;
    [SerializeField]
    private DialogueConfigHasCaughtOtherFish m_hasCaughtOtherFishDialogues;
    
    private FishRender m_fishRender;
    private FishState m_fishState = FishState.Idle;
    
    private Vector3 m_lastPosition;
    private float m_timeSinceIdleDialogue = 0;
    private float m_waitBeforeIdleDialogue = 0;
    private float m_runawayTime = 0;
    private Vector3 m_runawayDirection;

    public void Awake()
    {
        m_fishRender = GetComponentInChildren<FishRender>();
        m_fishRender.SetFishInstance(this);
        SetState(FishState.Idle);
    }

    
    private void Update()
    {
        if (m_fishState != FishState.Reeling)
        {
            LookTowardsMoveDirection();
        }

        if (m_fishState == FishState.Idle)
        {
            WaypointMovement();
            EvaluateIdleDialogue();
        }

        if (m_fishState == FishState.Runaway)
        {
            m_runawayTime += Time.deltaTime;
            RunawayMovement();
            
            if(m_runawayTime > 5f)
            {
                SetState(FishState.Idle);
            }
        }
    }

    public void SetState(FishState state)
    {
        m_fishRender.StopDialogue();
        m_runawayTime = 0;
        
        m_fishState = state;
        if (state == FishState.Idle)
        {
            m_timeSinceIdleDialogue = 0;
            m_waitBeforeIdleDialogue = Random.Range(2f, 10f);
        }
    }

    public void FishSeesHook(Transform hookPosition)
    {
        SetState(FishState.Runaway);
        m_runawayDirection = Vector3.Normalize(transform.position - hookPosition.position);
        EvaluateRunawayDialogue();
    }
    
    public void Rotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.Euler(rotate);
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
    
    private void RunawayMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + m_runawayDirection, m_speed * Time.deltaTime);
    }
    
    private void LookTowardsMoveDirection()
    {
        Vector3 newPosition = transform.position;
        Vector3 moveDirection = newPosition - m_lastPosition;
        if (moveDirection.x > 0)
        {
            Rotate(new Vector3(0, 180, 0));
        }
        else
        {
            Rotate(new Vector3(0, 0, 0));
        }

        m_lastPosition = transform.position;
    }

    private void EvaluateIdleDialogue()
    {
        m_timeSinceIdleDialogue += Time.deltaTime;
        if (m_timeSinceIdleDialogue > m_waitBeforeIdleDialogue)
        {
            m_timeSinceIdleDialogue = 0;
            m_waitBeforeIdleDialogue = Random.Range(5f, 10f);
            string dialogue = m_idleDialogues.GetDialogue();
            if (!string.IsNullOrEmpty(dialogue))
            {
                PlayDialogue(dialogue);
            }
        }
    }

    private void EvaluateRunawayDialogue()
    {
        List<string> validConfigs = new List<string>(m_dialogueConfigRunaways.GetValidDialogues());
        validConfigs.AddRange(m_hasCaughtOtherFishDialogues.GetValidDialogues());
        
        if(validConfigs.Count == 0)
        {
            return;
        }
        
        string dialogueConfig = validConfigs[Random.Range(0, validConfigs.Count)];
        PlayDialogue(dialogueConfig);
    }

    private List<DialogueItem> GetDialogueWhenCaught()
    {
        return m_fishWasCaughtDialogues.Conversation;
    }

    public void PlayDialogue(string dialogue)
    {
        m_fishRender.PlayDialogue(dialogue);
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
}
