using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    public GameObject prefab;

    public void InstantiatePrefab()
    {
        GameObject go = GameObject.Instantiate(prefab,transform);
        go.transform.parent = null;
    }
}
