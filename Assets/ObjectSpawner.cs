using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawn objects in a sphere around this gameobject
public class ObjectSpawner : MonoBehaviour
{
    public List<PotentialObjectClass> PotentialObjects = new List<PotentialObjectClass>();

    public float spawnSphereRadius = 15;
    public float SpawnRate = 2f;
    public float SpawnRateVariation = .5f;


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
                Vector3 pos = new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f), Random.Range(1f, -1f));
                pos.Normalize();
                pos = pos * spawnSphereRadius;

                //To-do: add object pooling
                GameObject go = GameObject.Instantiate(obj.Prefab, transform);
                go.transform.localPosition = pos;
                go.transform.rotation = Random.rotation;

                yield return new WaitForSeconds(SpawnRate + Random.Range(SpawnRateVariation, -SpawnRateVariation));
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
}
