using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a base class to be inherited by any singleton classes, or classes that should have only one instance of themselves in an application.*/
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();

            return instance;
        }
    }
}
