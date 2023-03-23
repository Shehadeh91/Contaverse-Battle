using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Object Pool")]
public class Objectpool : ScriptableObject
{
    public GameObject[] objects;

    public GameObject GetRandomObjectFromPool()
    {
        if(objects.Length != 0)
            return objects[Random.Range(0, objects.Length)];
        return null;
    }
}