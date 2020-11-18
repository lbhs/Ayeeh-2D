using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawn objects in a sphere around this gameobject
public class ObjectSpawner : MonoBehaviour
{
    public List<PotentialObjectClass> PotentialObjects = new List<PotentialObjectClass>();

    [Header("Rate of Spawn")]
    public float TimeInBetweenSpawns = 2f;
    public float SpawnRateVariation = .5f;

    [Header("Where to Spawn")]
    [Tooltip("If 0, objects will spawn in a circle, but 1 would give you a sphere")]
    public float ZScaleVariation = 1;
    [Tooltip("Objects fly towards the center, use this to vary that")]
    public float ObjectTargetVariation = 3;

    [Header("Sphere properties")]
    public float spawnSphereRadius = 15;
    public Color GizmosColor = new Color(1, 0, 0.9953942f, 0.08627451f);

    void Start()
    {
        StartCoroutine("SpawnObj");
    }

    //loops every so often
    IEnumerator SpawnObj()
    {
        while (true == true) //will run forever 
        {
            PotentialObjectClass obj = ChooseRandomObj();

            if (obj != null)
            {
                //Random position on the 0 Z-plane
                Vector3 pos = new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f), Random.Range(ZScaleVariation, -ZScaleVariation));
                pos.Normalize();
                pos = pos * spawnSphereRadius;

                //To-do: add object pooling
                GameObject go = obj.SpawnParticle(transform.position, transform.rotation, transform);
                go.transform.localPosition = pos;
                go.transform.rotation = Random.rotation;

                //Set up moving the object
                if (go.GetComponent<FlyingObjectScript>() != null)
                {
                    Vector3 randPos = new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f), Random.Range(1f, -1f)) * ObjectTargetVariation;
                    go.GetComponent<FlyingObjectScript>().TargetPosition = transform.position + randPos;
                }

                yield return new WaitForSeconds(TimeInBetweenSpawns + Random.Range(SpawnRateVariation, -SpawnRateVariation));
            }
        }
    }

    PotentialObjectClass ChooseRandomObj()
    {
        PotentialObjectClass selectedObject = null;

        //get total weight
        float TotalWeight = 0;
        foreach (var item in PotentialObjects)
        {
            if (item.TimeUntilSpawn <= Time.time)//wait to introduce some objects
            {
                TotalWeight = TotalWeight + item.weight;
            }
        }

        if (TotalWeight == 0) { return null; }

        //Weighted random
        float randNum = Random.Range(0, TotalWeight);

        foreach (var item in PotentialObjects)
        {
            if (randNum < item.weight)
            {
                selectedObject = item;
                break; //exit foreach loop if we have selected an object
            }
            randNum = randNum - item.weight;
        }

        //done
        return selectedObject;
    }

    //visualization of spawning area
    private void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawSphere(transform.position, spawnSphereRadius + 1);
    }
}

[System.Serializable]
public class PotentialObjectClass
{
    [Tooltip("The object that will spawn")]
    public GameObject Prefab;
    [Tooltip("Higher values make this object spawn more often")]
    public int weight = 1;
    [Tooltip("These won't show up until x seconds after")]
    public int TimeUntilSpawn;

    [Tooltip("The amount of objects that can be on the scene at one time")]
    public int poolSize = 30;

    [HideInInspector]
    public List<Transform> objects = new List<Transform>();
    private int counter = 0;

    //object pooling (not spawning/destorying all the time (reuseing))
    public GameObject SpawnParticle(Vector3 pos, Quaternion quat, Transform parent = null)
    {
        GameObject go = null;
        if (objects.Count <= poolSize)
        {
            go = GameObject.Instantiate(Prefab, pos, quat, parent);
            objects.Add(go.transform);
        }
        else
        {
            if (counter == poolSize)
            {
                counter = 0;
            }
            else
            {
                counter++;
            }
            objects[counter].position = pos;
            objects[counter].rotation = quat;
            objects[counter].gameObject.SetActive(true);
            go = objects[counter].gameObject;
        }
        return go;
    }
}
