using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Tweakables")]
    
    [SerializeField] Transform target;
    [SerializeField] float smoothing = 0.1f;

    [Header("Behavior Type")]
    //Purely for Editor to decide if scene offset should be included
    [SerializeField] bool includeOffset = false;
    [SerializeField] bool startFollowing = false;

    public void ChangeTarget(Transform _target) => target = _target;

    Vector3 offset;
    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        if(includeOffset)
            offset = transform.position - target.position;
        else
            offset = Vector3.zero;

        //Z Offset should be const to make sure camera renders everything
        offset.z = -10;
    }


    void FixedUpdate()
    {

        
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

    }
}
