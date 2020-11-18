using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class FlyingObjectScript : MonoBehaviour
{
    [HideInInspector]
    public Vector3 TargetPosition;
    private Rigidbody rb;

    public float speed = 1;
    public float variationInSpeed = 0;

    public bool RandomAngularVelocity = true;

    private ParticleManager particleToPlay;

    //public UnityEvent OnHit;

    // Start is called before the first frame update
    void Start()
    {
        //set up variables
        rb = GetComponent<Rigidbody>();
        particleToPlay = GameObject.FindObjectOfType<ParticleManager>();

        //calculate direction
        TargetPosition = (TargetPosition - transform.position).normalized;

        //add variation and speed and grantee there is no zero or negative speed
        if (variationInSpeed > 0)
        {
            speed = speed + Random.Range(variationInSpeed, -variationInSpeed);
            if (speed <= 0) { speed = 1; }
        }

        //set angular velocity
        if (RandomAngularVelocity)
        {
            rb.angularVelocity = Random.insideUnitSphere;
        }

        //set velocity
        rb.velocity = TargetPosition * speed;
    }

    public void HitWithLazar()
    {
        //print("I got hit!" + gameObject.name);
        //OnHit.Invoke();
        particleToPlay.SpawnParticle(transform.position, Quaternion.identity);
        
        gameObject.SetActive(false);
    }
}
