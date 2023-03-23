using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGenericSingleton<T> : GenericSingleton<T> where T : Component
{
    public override void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            GenericSingletonManager.OnDestroyAllSingletons += ()=> Destroy(instance.gameObject);
            InitializeSingleton();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"There already exists a {this.GetType().ToString()} in this scene. This instance {gameObject.name} will be deleted.");
            Destroy(gameObject); 
        }
    }
}