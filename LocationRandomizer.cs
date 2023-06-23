using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRandomizer : MonoBehaviour
{
    public int range = 1;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 random_location = Random.insideUnitSphere * range;
        random_location.z = 0;
        transform.Translate(random_location);
    }
}
