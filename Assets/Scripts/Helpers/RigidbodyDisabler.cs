using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyDisabler : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField][Tooltip("Set to -1 if the rigidbody should be enabled indefinitely as long as the minimum velocity isn't reached.")]
    private bool beginCheckingOnStart = true;
    [SerializeField][Tooltip("Set to -1 if the rigidbody should be enabled indefinitely as long as the minimum velocity isn't reached.")]
    private float minEnabledTime = 1f;
    [SerializeField][Tooltip("Set to -1 if the rigidbody should be enabled indefinitely as long as the minimum velocity isn't reached.")]
    private float maxEnabledTime = 10f;
    [SerializeField]
    private float checkFrequency = 0.1f;
    private bool checksIndefinitely = false;
    
    [Header("Events")]
    [SerializeField]
    private UnityEvent onStoppedMoving = new UnityEvent();
    [SerializeField]
    private UnityEvent onTimeEnded = new UnityEvent();
    private Rigidbody rbody;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();
        if(maxEnabledTime < 0.1f)
            checksIndefinitely = true;
        if(beginCheckingOnStart)
            StartCoroutine(CheckRbodyVelocity());
    }

    public void StartChecking()
    {
        StartCoroutine(CheckRbodyVelocity());
    }

    private IEnumerator CheckRbodyVelocity()
    {
        // Debug.Log("Started checking", gameObject);
        float startTime = Time.time;
        yield return new WaitForSeconds(minEnabledTime);

        while(true)
        {
            yield return new WaitForSeconds(checkFrequency);

            if(rbody.velocity.sqrMagnitude < 0.1f && rbody.angularVelocity.sqrMagnitude < 1.2f)
            {
                // Debug.Log("stopped moving", gameObject);
                rbody.isKinematic = true;
                onStoppedMoving.Invoke();
                break;
            }

            if(!checksIndefinitely && startTime - Time.time > maxEnabledTime)
            {
                // Debug.Log("Time ended");
                rbody.isKinematic = true;
                onTimeEnded.Invoke();
                break;
            }
        }
    }
}