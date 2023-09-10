using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    [SerializeField] PersistentManager prefab;
    static PersistentManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void ResetManagers()
    {
        if (instance == null) return;
        
        var prefab = instance.prefab;
        Destroy(instance.gameObject);
        instance = Instantiate(prefab);
    }
}
