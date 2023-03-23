using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueItem : ScriptableObject
{
    private int priority; 
    public int GetPriority()
    {
        return priority; 
    }
    public void SetPriority(int prio)
    {
        priority = prio; 
    }

    private delegate TResult Func<in T, out TResult>(T arg);  
}
