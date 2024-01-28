using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FishState
{
    Idle,
    Runaway,
    Reeling,
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
    
    private FishRender m_fishRender;
    private FishState m_fishState = FishState.Idle;
    private List<DialogueConfigWhenIdle> m_idleDialogues = new();
    private List<DialogueConfigFishWasCaught> m_fishWasCaughtDialogues = new();
    private List<DialogueConfigHasCaughtOtherFish> m_hasCaughtOtherFishDialogues = new();
    private List<DialogueConfigRunaway> m_dialogueConfigRunaways = new();
    private Vector3 m_lastPosition;
    private float m_timeSinceIdleDialogue = 0;
    private float m_waitBeforeIdleDialogue = 0;
    private float m_runawayTime = 0;
    private Vector3 m_runawayDirection;

    public void Awake()
    {
        m_fishRender = GetComponentInChildren<FishRender>();
        m_fishRender.SetFishInstance(this);
        CollectDialogueConfigs();
        SetState(FishState.Idle);
    }

    private void CollectDialogueConfigs()
    {
        m_idleDialogues = GetComponents<DialogueConfigWhenIdle>().ToList();
        m_fishWasCaughtDialogues = GetComponents<DialogueConfigFishWasCaught>().ToList();
        m_hasCaughtOtherFishDialogues = GetComponents<DialogueConfigHasCaughtOtherFish>().ToList();
        m_dialogueConfigRunaways = GetComponents<DialogueConfigRunaway>().ToList();
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
        if(m_idleDialogues.Count == 0)
        {
            return;
        }
        
        m_timeSinceIdleDialogue += Time.deltaTime;
        if (m_timeSinceIdleDialogue > m_waitBeforeIdleDialogue)
        {
            m_timeSinceIdleDialogue = 0;
            m_waitBeforeIdleDialogue = Random.Range(5f, 10f);
            DialogueConfigWhenIdle dialogueConfig = m_idleDialogues[Random.Range(0, m_idleDialogues.Count)];
            m_fishRender.PlayDialogue(dialogueConfig.DialogueText);
        }
    }

    private void EvaluateRunawayDialogue()
    {
        if(m_dialogueConfigRunaways.Count == 0 && m_hasCaughtOtherFishDialogues.Count == 0)
        {
            return;
        }

        List<DialogueConfig> validConfigs = new List<DialogueConfig>(m_dialogueConfigRunaways);
        foreach(DialogueConfigHasCaughtOtherFish dialogueConfigToCheck in m_hasCaughtOtherFishDialogues)
        {
            if(dialogueConfigToCheck.IsConditionMet())
            {
                validConfigs.Add(dialogueConfigToCheck);
            }
        }
        
        if(validConfigs.Count == 0)
        {
            return;
        }
        
        DialogueConfig dialogueConfig = validConfigs[Random.Range(0, validConfigs.Count)];
        m_fishRender.PlayDialogue(dialogueConfig.DialogueText);
    }

    private List<DialogueItem> GetDialogueWhenCaught()
    {
        if(m_fishWasCaughtDialogues.Count == 0)
        {
            return null;
        }
        
        DialogueConfigFishWasCaught dialogueConfig = m_fishWasCaughtDialogues[Random.Range(0, m_fishWasCaughtDialogues.Count)];
        return dialogueConfig.Dialogues;
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
