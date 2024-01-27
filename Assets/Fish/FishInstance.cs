using System;
using System.Collections;
using UnityEngine;

public class FishInstance : MonoBehaviour
{
    [SerializeField]
    private FishDefinition m_fishDefinition;
    [SerializeField]
    public string m_guid = Guid.NewGuid().ToString();
    [SerializeField]
    private string m_fishName;

    private FishRender m_fishRender;

    public void Awake()
    {
        m_fishRender = GetComponentInChildren<FishRender>();
        StartCoroutine(PlayDialogueAfterWait());
    }

    private Vector3 destination;
    private float speed = 2f;
    
    private void Update()
    {
        SimpleMovement();
    }

    private void SimpleMovement()
    {
        // get new point
        if (Vector3.Magnitude(transform.position - destination) < 0.01f)
        {
            float randomX = UnityEngine.Random.Range(-10f, 10f);
            destination = new Vector3(randomX, 0, 0);
            //Debug.Log($"New Destination: {destination}");
        }
        
        // move towards point
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // Inefficient to do this every frame but... eh.
        bool isMovingLeft = (transform.position.x - destination.x) < 0;
        if(isMovingLeft)
            m_fishRender.RotateLeft();
        else
            m_fishRender.RotateRight();
    }
    
    private IEnumerator PlayDialogueAfterWait()
    {
        yield return new WaitForSeconds(3);
        m_fishRender.PlayDialogue("tf you looking at?");
    }
}
