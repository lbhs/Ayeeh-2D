using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
#if UNITY_EDITOR
                Debug.DrawLine(ray.origin, hit.point);
#endif

                if (hit.transform.GetComponent<FlyingObjectScript>() != null)
                {
                    FlyingObjectScript obj = hit.transform.GetComponent<FlyingObjectScript>();
                    obj.HitWithLazar();
                }
            }
        }
    }
}
