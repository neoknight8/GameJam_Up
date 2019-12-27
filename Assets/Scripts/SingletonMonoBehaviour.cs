using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
            }
            if (instance == null)
            {
                instance = CreateInstance();
            }
            return instance;
        }
    }

    public static bool HasInstance()
    {
        return instance != null;
    }

    private static T CreateInstance()
    {
        var obj = new GameObject(typeof(T).Name);
        instance = obj.AddComponent<T>();
        return instance;
    }

    private void OnDestroy()
    {
        if (instance == null)
        {
            return;
        }
        if (instance.gameObject == null)
        {
            return;
        }
        GameObject.Destroy(instance);
        instance = null;
    }
}