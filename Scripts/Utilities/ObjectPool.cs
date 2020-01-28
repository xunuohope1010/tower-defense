using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;

    private List<GameObject> pooledObjects = new List<GameObject>();

    public GameObject GetObject(string type)
    {
        // if the pool does contain the requested object, reuse it to maximize efficiency
        foreach (GameObject go in pooledObjects)
        {
            if (go.name == type && !go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        // if the pool does not contain the requested object, then create it into the pool
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }

        return null;
    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
