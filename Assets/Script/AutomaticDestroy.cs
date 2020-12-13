using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDestroy : MonoBehaviour
{
    [SerializeField] float destructionTime = 0.5f;
    float creationTime;

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - creationTime >= destructionTime)
        {
            Destroy(this);
        }
    }
}
