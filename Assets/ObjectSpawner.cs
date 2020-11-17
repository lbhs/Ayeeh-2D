using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public List<PotentialObjectClass> PotentialObjects = new List<PotentialObjectClass>();

    private int TotalWeight = 0;

    public float SpawnRate = 2f;
    public float Variation = .5f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in PotentialObjects)
        {
            TotalWeight += item.weight;
        }
        foreach (var item in PotentialObjects)
        {
            StartCoroutine("SpawnObj", item);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnObj(PotentialObjectClass obj)
    {
        while (true == true)
        {
            if (obj.TimeUntilSpawn > Time.time)
            {
                Vector3 pos = new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f), 0);
                pos.Normalize();
                pos = pos * 15f;
                GameObject.Instantiate(obj.Prefab, pos, Random.rotation); //To-do: add object pooling
                yield return new WaitForSeconds(SpawnRate + Random.Range(Variation, -Variation));
            }
        }
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
