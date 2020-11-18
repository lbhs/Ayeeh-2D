using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> particles = new List<Transform>();
    private int counter;
    public GameObject prefab;
    public int poolSize = 10;

    public void SpawnParticle(Vector3 pos, Quaternion quat)
    {
        if (particles.Count <= 10)
        {
            GameObject go = GameObject.Instantiate(prefab, pos, quat, transform);
            particles.Add(go.transform);
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
            particles[counter].position = pos;
            particles[counter].rotation = quat;
            particles[counter].GetComponent<ParticleSystem>().Play();
        }
    }
}
